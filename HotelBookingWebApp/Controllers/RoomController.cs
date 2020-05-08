using HotelBookingWebApp.Models;
using HotelBookingWebApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace HotelBookingWebApp.Controllers
{
    public class RoomController : Controller
    {
        HotelBookingDBEntities db = new HotelBookingDBEntities();
        RoomViewModel objRoomViewModel = new RoomViewModel();
        public RoomController()
        {

        }
        public ActionResult Index()
        {
            objRoomViewModel.ListOfBookingStatus = (from d in db.BookingStatus
                                                    select new SelectListItem()
                                                    {
                                                        Text=d.BookingStatus,
                                                        Value=d.BookingStatusId.ToString()
                                                    }).ToList();
            objRoomViewModel.ListOfRoomType= (from d in db.RoomTypes
                                              select new SelectListItem()
                                              {
                                                  Text = d.RoomTypeName,
                                                  Value = d.RoomTypeId.ToString(),
                                              }).ToList();
            return View(objRoomViewModel);
        }
        [HttpPost]
        public JsonResult Index(RoomViewModel objRoomViewModel)
                {
            string message = String.Empty;
            string ImageUniqueName = String.Empty;
            string ActualImageRoom =String.Empty;
            if (objRoomViewModel.RoomId==0)
            {
                ImageUniqueName = Guid.NewGuid().ToString();  // GUID identificador unico y global. hace una nueva instancia de guid
                ActualImageRoom = ImageUniqueName + Path.GetExtension(objRoomViewModel.Image.FileName);
                objRoomViewModel.Image.SaveAs(Server.MapPath("~/RoomImagens/" + ActualImageRoom));



                Room objRoom = new Room()
                {
                    RoomCapacity = objRoomViewModel.RoomCapacity,
                    RoomDescription = objRoomViewModel.RoomDescription,
                    RoomPrice = objRoomViewModel.RoomPrice,
                    BookingStatusId = objRoomViewModel.BookingStatusId,
                    RoomImage = ActualImageRoom,
                    IsActive = true,
                    RoomNumber = objRoomViewModel.RoomNumber,
                    RoomTypeId = objRoomViewModel.RoomTypeId,


                };
                db.Rooms.Add(objRoom);
                message = "Added.";
            }
            else
            {
                Room objRoom = db.Rooms.Single(model => model.RoomId == objRoomViewModel.RoomId);
                if (objRoomViewModel.Image != null)
                {
                    ImageUniqueName = Guid.NewGuid().ToString();  // GUID identificador unico y global. hace una nueva instancia de guid
                    ActualImageRoom = ImageUniqueName + Path.GetExtension(objRoomViewModel.Image.FileName);
                    objRoomViewModel.Image.SaveAs(Server.MapPath("~/RoomImagens/" + ActualImageRoom));
                    objRoom.RoomImage = ActualImageRoom;
                }
                objRoom.RoomCapacity = objRoomViewModel.RoomCapacity;
                objRoom.RoomDescription = objRoomViewModel.RoomDescription;
                objRoom.RoomPrice = objRoomViewModel.RoomPrice;
                objRoom.BookingStatusId = objRoomViewModel.BookingStatusId;
                objRoom.IsActive = true;
                objRoom.RoomNumber = objRoomViewModel.RoomNumber;
                objRoom.RoomTypeId = objRoomViewModel.RoomTypeId;
                message = "Update.";
            }
                db.SaveChanges();
            

            return Json(new { message="Reservacion "+message, success=true}, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetAllRooms()
        {
            IEnumerable<RoomDetailsViewModel> listOfRoomDetailsViewModel = (from d in db.Rooms
                                                                            join objBooking in db.BookingStatus on d.BookingStatusId equals objBooking.BookingStatusId
                                                                            join objRoomType in db.RoomTypes on d.RoomTypeId equals objRoomType.RoomTypeId
                                                                            where d.IsActive == true
                                                                            select new RoomDetailsViewModel()
                                                                            {
                                                                                RoomNumber = d.RoomNumber,
                                                                                RoomCapacity = d.RoomCapacity,
                                                                                RoomImage = d.RoomImage,
                                                                                RoomPrice = d.RoomPrice,
                                                                                BookingStatus = objBooking.BookingStatus,
                                                                                RoomType = objRoomType.RoomTypeName,
                                                                                RoomId = d.RoomId

                                                                            }).ToList();

            return PartialView("_RoomDetailsPartial", listOfRoomDetailsViewModel);
        }
        [HttpGet]
        public JsonResult EditRoomDetails(int RoomId)
        {
            var result = db.Rooms.Single(d => d.RoomId == RoomId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteRoomDetails(int RoomId)
        {
            Room objRoom= db.Rooms.Single(d => d.RoomId == RoomId);
            objRoom.IsActive = false;
            db.SaveChanges();

            return Json(new { Message="Borrado con exito", Success=true} , JsonRequestBehavior.AllowGet);
        }

    }
}