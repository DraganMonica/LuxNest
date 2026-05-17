using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using LuxNest.Application.Common.Interfaces;
using LuxNest.Application.Common.Utility;
using LuxNest.Application.Services.Interface;
using LuxNest.Domain.Entities;

namespace LuxNest.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVillaService _villaService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVillaNumberService _villaNumberService;

        public BookingController(IBookingService bookingService, IWebHostEnvironment webHostEnvironment, 
            IVillaService villaService, IVillaNumberService villaNumberService, 
            UserManager<ApplicationUser> userManager)
        {
            _bookingService = bookingService;
            _webHostEnvironment = webHostEnvironment;
            _villaService = villaService;
            _userManager = userManager;
            _villaNumberService = villaNumberService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult MyBookings()
        {
            return View();
        }

        [Authorize]
        public IActionResult FinalizeBooking(int villaId, DateOnly checkInDate, int nights, int numberOfRooms = 1, int numberOf2pRooms = 1, int numberOf3pRooms = 0, int numberOfGuests = 1)
        {
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var userId=claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user=_userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            var villa = _villaService.GetVillaById(villaId);

            Booking booking = new()
            {
                VillaId = villaId,
                Villa = villa,
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name,
                NumberOfRooms = numberOfRooms,
                NumberOfGuests = numberOfGuests,
                CreatedAt = DateTime.Now
            };
            var checkedIn = _bookingService.GetCheckedInVillaNumbers(villaId);
            var available = _villaNumberService.GetAllVillaNumbers()
                .Where(vn => vn.VillaId == villaId && !checkedIn.Contains(vn.Villa_Number))
                .ToList();

            var rooms2p = available.Where(vn => vn.Capacity <= 2).Take(numberOf2pRooms).ToList();
            var rooms3p = available.Where(vn => vn.Capacity >= 3).Take(numberOf3pRooms).ToList();
            var selectedRooms = rooms2p.Concat(rooms3p).ToList();
            if (selectedRooms.Count < numberOfRooms)
            {
                var extra = available.Except(selectedRooms).Take(numberOfRooms - selectedRooms.Count);
                selectedRooms.AddRange(extra);
            }

            booking.TotalCost = selectedRooms.Sum(r => villa.Price + (r.Capacity - 2) * 50.0) * nights;
            ViewBag.MaxGuests = selectedRooms.Sum(r => r.Capacity);
            ViewBag.NumberOf2pRooms = rooms2p.Count;
            ViewBag.NumberOf3pRooms = rooms3p.Count;
            ViewBag.RoomBreakdown = string.Join("  +  ", new[]
            {
                rooms2p.Count > 0 ? $"{rooms2p.Count} × 2-person room{(rooms2p.Count > 1 ? "s" : "")} — ${villa.Price:N0}/night" : null,
                rooms3p.Count > 0 ? $"{rooms3p.Count} × 3-person room{(rooms3p.Count > 1 ? "s" : "")} — ${villa.Price + 50:N0}/night" : null
            }.Where(s => s != null));

            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking, int numberOf2pRooms = 1, int numberOf3pRooms = 0)
        {
            var villa = _villaService.GetVillaById(booking.VillaId);
            var checkedInForPost = _bookingService.GetCheckedInVillaNumbers(booking.VillaId);
            var availableForPost = _villaNumberService.GetAllVillaNumbers()
                .Where(vn => vn.VillaId == booking.VillaId && !checkedInForPost.Contains(vn.Villa_Number))
                .ToList();
            var post2p = availableForPost.Where(vn => vn.Capacity <= 2).Take(numberOf2pRooms).ToList();
            var post3p = availableForPost.Where(vn => vn.Capacity >= 3).Take(numberOf3pRooms).ToList();
            var selectedRoomsForPost = post2p.Concat(post3p).ToList();
            if (selectedRoomsForPost.Count < booking.NumberOfRooms)
            {
                var extra = availableForPost.Except(selectedRoomsForPost).Take(booking.NumberOfRooms - selectedRoomsForPost.Count);
                selectedRoomsForPost.AddRange(extra);
            }
            booking.TotalCost = selectedRoomsForPost.Sum(r => villa.Price + (r.Capacity - 2) * 50.0) * booking.Nights;

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            if (booking.NumberOfRooms < 1)
                booking.NumberOfRooms = 1;

            if (selectedRoomsForPost.Count < booking.NumberOfRooms)
            {
                TempData["error"] = $"Only {availableForPost.Count} room(s) available for the selected dates.";
                return RedirectToAction(nameof(FinalizeBooking), new
                {
                    villaId = booking.VillaId,
                    checkInDate = booking.CheckInDate.ToString("yyyy-MM-dd"),
                    nights = booking.Nights,
                    numberOfRooms = booking.NumberOfRooms,
                    numberOf2pRooms,
                    numberOf3pRooms,
                    numberOfGuests = booking.NumberOfGuests
                });
            }

            int maxGuests = selectedRoomsForPost.Sum(vn => vn.Capacity);
            if (booking.NumberOfGuests < 1 || booking.NumberOfGuests > maxGuests)
            {
                TempData["error"] = $"The selected {booking.NumberOfRooms} room(s) can accommodate maximum {maxGuests} guests.";
                return RedirectToAction(nameof(FinalizeBooking), new
                {
                    villaId = booking.VillaId,
                    checkInDate = booking.CheckInDate.ToString("yyyy-MM-dd"),
                    nights = booking.Nights,
                    numberOfRooms = booking.NumberOfRooms,
                    numberOf2pRooms,
                    numberOf3pRooms,
                    numberOfGuests = booking.NumberOfGuests
                });
            }

            if(!_villaService.IsVillaAvailableByDate(villa.Id, booking.Nights, booking.CheckInDate))
            {
                TempData["error"] = "Room has been sold out!";
                return RedirectToAction(nameof(FinalizeBooking), new
                {
                    villaId=booking.VillaId,
                    checkInDate=booking.CheckInDate,
                    nights=booking.Nights
                });
            }

            _bookingService.CreateBooking(booking);

            //
            var domain = Request.Scheme + "://" +Request.Host.Value+"/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode ="payment",
                SuccessUrl=domain + $"booking/BookingConfirmation?bookingId={booking.Id}",
                CancelUrl=domain + $"booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
            };

            options.LineItems.Add(new SessionLineItemOptions {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalCost * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name,
                        //Images=new List<string> { domain+villa.ImageUrl},
                    },
                },
                Quantity = 1,
            });

            var service = new SessionService();
            Session session=service.Create(options);

            _bookingService.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);

            if (bookingFromDb.Status == SD.StatusPending)
            {
                var service = new SessionService();
                Session session = service.Get(bookingFromDb.StripeSessionId);
                if (session.PaymentStatus == "paid")
                {
                    var availableRooms = AssignAvailableVillaNumberByVilla(bookingFromDb.VillaId);
                    int assignedRoom = availableRooms.FirstOrDefault();
                    _bookingService.UpdateStatus(bookingFromDb.Id, SD.StatusApproved, assignedRoom);
                    _bookingService.UpdateStripePaymentID(bookingFromDb.Id, session.Id, session.PaymentIntentId);
                }
            }
            return View(bookingId);
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId) {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);

            if (bookingFromDb.Villa != null)
            {
                var allVillaNumbers = _villaNumberService.GetAllVillaNumbers()
                    .Where(vn => vn.VillaId == bookingFromDb.VillaId).ToList();
                bookingFromDb.Villa.Rooms2p = allVillaNumbers.Count(vn => vn.Capacity <= 2);
                bookingFromDb.Villa.Rooms3p = allVillaNumbers.Count(vn => vn.Capacity >= 3);
                bookingFromDb.Villa.TotalCapacity = allVillaNumbers.Sum(vn => vn.Capacity);
            }

            if (bookingFromDb.VillaNumber == 0 && bookingFromDb.Status == SD.StatusApproved)
            {
                var availableVillaNumber = AssignAvailableVillaNumberByVilla(bookingFromDb.VillaId);
                bookingFromDb.VillaNumbers = _villaNumberService.GetAllVillaNumbers()
                    .Where(u => u.VillaId == bookingFromDb.VillaId && availableVillaNumber.Any(x => x == u.Villa_Number))
                    .ToList();
            }

            if (bookingFromDb.VillaNumber > 0 &&
                (bookingFromDb.Status == SD.StatusCheckedIn || bookingFromDb.Status == SD.StatusCompleted))
            {
                bookingFromDb.VillaNumbers = _villaNumberService.GetAllVillaNumbers()
                    .Where(u => u.Villa_Number == bookingFromDb.VillaNumber)
                    .ToList();
            }

            return View(bookingFromDb);
        }

        [Authorize]
        public IActionResult GenerateInvoice(int id, string downloadType)
        {
            string basePath = _webHostEnvironment.WebRootPath;
            WordDocument document = new WordDocument();

            //Load the template
            string dataPath = basePath + @"/exports/BookingDetails.docx";
            using FileStream fileStream = new (dataPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            document.Open(fileStream, Syncfusion.DocIO.FormatType.Automatic);

            //Update template
            Booking bookingFromDb = _bookingService.GetBookingById(id);

            TextSelection textSelection = document.Find("xx_customer_name", false, true);
            WTextRange textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Name;

            textSelection = document.Find("xx_customer_phone", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Phone;

            textSelection = document.Find("xx_customer_email", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Email;

            textSelection = document.Find("XX_BOOKING_NUMBER", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = "BOOKING ID - "+bookingFromDb.Id;

            textSelection = document.Find("XX_BOOKING_DATE", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = "BOOKING DATE - " + bookingFromDb.BookingDate.ToShortDateString();

            textSelection = document.Find("xx_payment_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.PaymentDate.ToShortDateString();

            textSelection = document.Find("xx_checkin_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.CheckInDate.ToShortDateString();

            textSelection = document.Find("xx_checkout_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.CheckOutDate.ToShortDateString();

            textSelection = document.Find("xx_booking_total", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.TotalCost.ToString("N2");

            WTable table = new(document);

            table.TableFormat.Borders.LineWidth = 1f;
            table.TableFormat.Borders.Color = Syncfusion.Drawing.Color.Black;
            table.TableFormat.Paddings.Top = 7f;
            table.TableFormat.Paddings.Bottom = 7f;
            table.TableFormat.Borders.Horizontal.LineWidth = 1f;

            int rows = bookingFromDb.VillaNumber > 0 ? 3 : 2;
            table.ResetCells(rows, 4);

            WTableRow row0=table.Rows[0];

            row0.Cells[0].AddParagraph().AppendText("NIGHTS");
            row0.Cells[0].Width = 80;
            row0.Cells[1].AddParagraph().AppendText("VILLA");
            row0.Cells[1].Width = 80;
            row0.Cells[2].AddParagraph().AppendText("PRICE PER NIGHT");
            row0.Cells[3].AddParagraph().AppendText("TOTAL");
            row0.Cells[3].Width = 80;

            WTableRow row1=table.Rows[1];

            row1.Cells[0].AddParagraph().AppendText(bookingFromDb.Nights.ToString());
            row1.Cells[0].Width = 80;
            row1.Cells[1].AddParagraph().AppendText(bookingFromDb.Villa.Name);
            row1.Cells[1].Width = 80;
            row1.Cells[2].AddParagraph().AppendText((bookingFromDb.TotalCost/bookingFromDb.Nights).ToString("N2"));
            row1.Cells[3].AddParagraph().AppendText(bookingFromDb.TotalCost.ToString("N2"));
            row1.Cells[3].Width = 80;

            if (bookingFromDb.VillaNumber > 0)
            {
                WTableRow row2 = table.Rows[2];

                row2.Cells[0].Width = 80;
                row2.Cells[1].AddParagraph().AppendText("Villa Number - " + bookingFromDb.VillaNumber.ToString());
                row2.Cells[1].Width = 220;
                row2.Cells[3].Width = 80;
            }

            WTableStyle tableStyle = document.AddTableStyle("CustomStyle") as WTableStyle;
            tableStyle.TableProperties.RowStripe = 1;
            tableStyle.TableProperties.ColumnStripe = 2;
            tableStyle.TableProperties.Paddings.Top = 2;
            tableStyle.TableProperties.Paddings.Bottom = 1;
            tableStyle.TableProperties.Paddings.Left =5.4f;
            tableStyle.TableProperties.Paddings.Right = 5.4f;

            ConditionalFormattingStyle firstRowStyle = tableStyle.ConditionalFormattingStyles.Add(ConditionalFormattingType.FirstRow);
            firstRowStyle.CharacterFormat.Bold = true;
            firstRowStyle.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(255,255,255,255);
            firstRowStyle.CellProperties.BackColor = Syncfusion.Drawing.Color.Black;

            table.ApplyStyle("CustomStyle");

            TextBodyPart bodyPart = new(document);
            bodyPart.BodyItems.Add(table);

            document.Replace("<ADDTABLEHERE>", bodyPart, false, false);


            using DocIORenderer renderer = new();
            MemoryStream stream = new();
            if (downloadType == "word")
            {
                document.Save(stream, Syncfusion.DocIO.FormatType.Docx);
                stream.Position = 0;

                return File(stream, "application/docx", "BookingDetails.docx");
            }
            else { 
                PdfDocument pdfDocument = renderer.ConvertToPDF(document);
                pdfDocument.Save(stream);
                stream.Position = 0;

                return File(stream, "application/pdf", "BookingDetails.pdf");
            }
        }

        [HttpPost]
        [Authorize(Roles =SD.Role_Admin)]
        public IActionResult CheckIn(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);

            TempData["Success"] = "Booking Updated Successfully";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckOut(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
 
            TempData["Success"] = "Booking Completed Successfully";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelBooking(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCancelled, 0);

            TempData["Success"] = "Booking Cancelled Successfully";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [Authorize]
        public IActionResult CancelBookingUser(int bookingId)
        {
            var bookingFromDb = _bookingService.GetBookingById(bookingId);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (bookingFromDb.UserId != userId)
            {
                TempData["Error"] = "You are not authorized to cancel this booking.";
                return RedirectToAction(nameof(MyBookings));
            }

            if (bookingFromDb.Status != SD.StatusPending && bookingFromDb.Status != SD.StatusApproved)
            {
                TempData["Error"] = "This booking cannot be cancelled.";
                return RedirectToAction(nameof(MyBookings));
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            int daysUntilCheckIn = bookingFromDb.CheckInDate.DayNumber - today.DayNumber;

            if (bookingFromDb.IsPaymentSuccessful && !string.IsNullOrEmpty(bookingFromDb.StripePaymentIntentId))
            {
                double refundPercent = daysUntilCheckIn > 7 ? 1.0 : daysUntilCheckIn >= 3 ? 0.5 : 0.0;
                double refundAmount = bookingFromDb.TotalCost * refundPercent;

                if (refundPercent > 0)
                {
                    var refundService = new Stripe.RefundService();
                    var refundOptions = new Stripe.RefundCreateOptions
                    {
                        PaymentIntent = bookingFromDb.StripePaymentIntentId,
                        Amount = (long)(refundAmount * 100)
                    };
                    refundService.Create(refundOptions);

                    _bookingService.UpdateStatus(bookingId, SD.StatusRefunded, 0);
                    TempData["Success"] = $"Booking cancelled. You will receive a {refundPercent * 100:0}% refund of ${refundAmount:N2} ({daysUntilCheckIn} days before check-in).";
                }
                else
                {
                    _bookingService.UpdateStatus(bookingId, SD.StatusCancelled, 0);
                    TempData["Success"] = "Booking cancelled. No refund applies — cancellation is less than 3 days before check-in.";
                }
            }
            else
            {
                _bookingService.UpdateStatus(bookingId, SD.StatusCancelled, 0);
                TempData["Success"] = "Booking cancelled successfully.";
            }

            return RedirectToAction(nameof(MyBookings));
        }

        private List<int> AssignAvailableVillaNumberByVilla(int villaId)
        {
            List<int> availableVillaNumbers = new();

            var villaNumbers = _villaNumberService.GetAllVillaNumbers().Where(u => u.VillaId == villaId);

            var takenRooms = _bookingService.GetAllBookings()
                .Where(u => u.VillaId == villaId &&
                    (u.Status == SD.StatusCheckedIn || u.Status == SD.StatusApproved) &&
                    u.VillaNumber > 0)
                .Select(u => u.VillaNumber)
                .ToHashSet();

            foreach (var villaNumber in villaNumbers)
            {
                if (!takenRooms.Contains(villaNumber.Villa_Number))
                {
                    availableVillaNumbers.Add(villaNumber.Villa_Number);
                }
            }

            return availableVillaNumbers;
        }

        #region API Calls
        [HttpGet]
        //[Authorize]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> objBookings;
            string userId = "";
            if (string.IsNullOrEmpty(status))
            {
                status = "";
            }
            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity=(ClaimsIdentity)User.Identity;
                userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            objBookings = _bookingService.GetAllBookings(userId, status);

            return Json(new {data=objBookings});
        }
        #endregion
    }
}
