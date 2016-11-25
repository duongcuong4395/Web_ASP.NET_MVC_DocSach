//Chặn nhập chữ chỉ cho nhập số 
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    else {
        return true;
    }
}

//không cho nhập quá số lượng của object
function maxLengthCheck(object) {
    var placeholder = object.placeholder;

    if (object.value.length > object.maxLength) {
        alert("Đã vượt quá số lượng cho phép");
        object.value = object.value.slice(0, object.maxLength);
    } 
}

function VietKhongDau(object) {
    //
    var str;
    if (eval(object))
        str = eval(object).value;
    else
        str = object;
    //str = str.toLowerCase();
    //chữ thường
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    //Chữ hoa
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ẳ|Ẵ|Ặ/g, "A");
    str = str.replace(/È|É|Ẻ|Ẽ|Ẹ|Ê|Ể|Ễ|Ề|Ế|Ệ/g, "E");
    str = str.replace(/Ì|Í|Ỉ|Ĩ|Ị/g, "I");
    str = str.replace(/Ò|Ó|Ỏ|Õ|Ọ|Ô|Ồ|Ố|Ổ|Ỗ|Ộ|Ơ|Ờ|Ớ|Ở|Ỡ|Ợ/g, "O");
    str = str.replace(/Ù|Ú|Ủ|Ũ|Ụ|Ư|Ừ|Ứ|Ử|Ữ|Ự/g, "U");
    str = str.replace(/Ỳ|Ý|Ỷ|Ỹ|Ỵ/g, "Y");
    str = str.replace(/Đ/g, "D");


    //str= str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g,"-");  
    /* tìm và thay thế các kí tự đặc biệt trong chuỗi sang kí tự - */
    //str= str.replace(/-+-/g,"-"); //thay thế 2- thành 1-  
    str = str.replace(/^\-+|\-+$/g, "");
    //cắt bỏ ký tự - ở đầu và cuối chuỗi 
    eval(object).value = str; 
}

function Vietmatkhau(object) {
    //
    var str;
    if (eval(object))
        str = eval(object).value;
    else
        str = object;
    //str = str.toLowerCase();
    //chữ thường
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    //Chữ hoa
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ẳ|Ẵ|Ặ/g, "A");
    str = str.replace(/È|É|Ẻ|Ẽ|Ẹ|Ê|Ể|Ễ|Ề|Ế|Ệ/g, "E");
    str = str.replace(/Ì|Í|Ỉ|Ĩ|Ị/g, "I");
    str = str.replace(/Ò|Ó|Ỏ|Õ|Ọ|Ô|Ồ|Ố|Ổ|Ỗ|Ộ|Ơ|Ờ|Ớ|Ở|Ỡ|Ợ/g, "O");
    str = str.replace(/Ù|Ú|Ủ|Ũ|Ụ|Ư|Ừ|Ứ|Ử|Ữ|Ự/g, "U");
    str = str.replace(/Ỳ|Ý|Ỷ|Ỹ|Ỵ/g, "Y");
    str = str.replace(/Đ/g, "D");


    //str= str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g,"-");  
    /* tìm và thay thế các kí tự đặc biệt trong chuỗi sang kí tự - */
    //str= str.replace(/-+-/g,"-"); //thay thế 2- thành 1-  
    str = str.replace(/^\-+|\-+$/g, "");
    //cắt bỏ ký tự - ở đầu và cuối chuỗi 
    eval(object).value = str;

}

 