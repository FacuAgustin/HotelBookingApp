$(document).ready(function () {
    LoadRoomDetails();
    $("#txtRoomId").val("0")
    $("#btnSave").click(function () {
        if (!$("#formRoom").valid()) {
            return;
        }
        SaveRoomData();
    });
});

function DeleteRoom(RoomId) {
    var result = confirm("Seguro que quiero borrar reserva?");
    if (result===false) {
        return false;
    }
    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        data: { RoomId: RoomId },
        url: '/Room/DeleteRoomDetails',
        success: function (data) {
            if (data.Success === true) {
                alert(data.Message);
                LoadRoomDetails();
            }
        },
        error: function () {
            alert("There is some problem to process your requets")
        }
    });
}

function EditRoom(RoomId) {
    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        data: { RoomId:RoomId },
        url: '/Room/EditRoomDetails',
        success: function (data) {
            $("#txtRoomNumber").val(data.RoomNumber);
            $("#txtRoomPrice").val(data.RoomPrice);
            $("#ddBookingStatus").val(data.BookingStatusId);
            $("#ddRoomType").val(data.RoomTypeId);
            $("#txtRoomCapacity").val(data.RoomCapacity);
            $("#txtRoomDescription").val(data.RoomDescription);
            $("#imagRoom").attr('src', "../RoomImagens/"+data.RoomImage);
            $("#divAddRoom").modal({ show: true });
            $("#txtRoomId").val(data.RoomId);
            $("#btnSave").text("Update");
        },
        error: function () {
            alert('No se pudo borrar la reserva');
        }
    });
}

function LoadRoomDetails() {
    $.ajax({
        async: true,
        data: 'GET',
        dataType: 'HTML',
        contentType: false,
        processData: false,
        url: '/Room/GetAllRooms',
        success: function (data) {
            $("#idRoomLoadDetails").html(data);
        },
        error: function () {
            alert("Hubo un error en el proceso de request");
        }

    })
}

function SaveRoomData() {   // funcion para salvar la seleccion de habitacion
    var formData = new FormData(); //creo variable para crear pares de clave/valor lista para mandar esa informacion.
    formData.append("RoomId", $("#txtRoomId").val());
    formData.append("RoomNumber", $("#txtRoomNumber").val());  //con .append mandamos pares de clave/valor al FormData()
    formData.append("RoomPrice", $("#txtRoomPrice").val());
    formData.append("BookingStatusId", $("#ddBookingStatus").val());
    formData.append("RoomTypeId", $("#ddRoomType").val());
    formData.append("RoomCapacity", $("#txtRoomCapacity").val());
    formData.append("RoomDescription", $("#txtRoomDescription").val());
    formData.append("Image", $("#UpLoadImg").get(0).files[0]);


    $.ajax({
        async: true,
        type: 'POST',
        dataType: 'JSON',
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data.success===true) {
                alert(data.message)
                ResertItem()
                
                LoadRoomDetails()
            }
        },
        error: function () {
            alert("No se pudo hacer la reserva");
        }

    })
}
function ResertItem() {
    $("#txtRoomNumber").val(''); 
    $("#txtRoomPrice").val('');
    $("#ddBookingStatus").val(1);
    $("#ddRoomType").val(1);
    $("#txtRoomCapacity").val('');
    $("#txtRoomDescription").val('');
    $("#UpLoadImg").focus();
    $("#imagRoom").removeAttr('src');
    $("#txtRoomId").val(0);
    $("#btnSave").val("save");

}

function DisplayImage(result) {
    if (result.files && result.files[0]) {
        var fileReader = new FileReader(); //permite a la app leer ficheros
        fileReader.onload = function (e) {
            $("#imagRoom").attr('src', e.target.result);  //.target.   Una referencia a un objeto que lanzo el evento
        }
        fileReader.readAsDataURL(result.files[0]);  //El método readAsDataURL es usado para leer el contenido del especificado File.
    }
}