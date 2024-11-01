// kết hợp với jquery from w3school
$(document).ready(function () {
    $(".btnAdd").click(function () {
        console.log("WTFFFFFFFFFFFFFFFFFFFFFFF");
        var value = $('#inputTodo').attr('value');
        console.log('value is: ', value);
        var newValue = $('#inputTodo').prop('value');
        console.log('new value is: ', newValue);
        var exactValue = $('#inputTodo').val();
        console.log('Same as value above: ', exactValue);
    });
    //without using object {key, value};
    $("#inputTodo").focus(function () {
        $(this).css('background-color', 'red');
    });
    // using object {'key':'value',...}
    $("#inputTodo").focus(function () {
        $(this).css({ 'background-color': 'red', 'color': 'blue' });
    });
    // option-select
    $('#test').change(function () {
        var chosen = $(this).val();
        console.log('selected option value is: ', chosen);
    });
    // gender
    $('.inputGender').change(function () {
        console.log('Changed into: ', $(this).val());
    });
    // 
    $('#inputTodo').keypress(function (e) {
        // if (e.keycode == 13){
        // => keycode = 13 nhưng khi code thì jquery sẽ phải
        //                                          sử dụng keyword "which"
        if (e.which == 13) {
            $('.btnAdd').click();
            // console.log(e);
        }
    });

    
});