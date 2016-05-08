

/*
    layer-2.2  重写alert
*/

/*初体验*/
function Alert(msg) {
    layer.alert(msg);
}
function Alert(msg, icon) {
    layer.alert(msg, {icon:icon});
}

/*提示层*/
function Msg(msg) {
    layer.msg(msg);
}
function Msg(msg,icon) {
    layer.msg(msg, { icon: icon });
}

/*询问框*/
function Confirm(msg, confirm, cancel, confirmResult, cancelResult) {
    layer.confirm(
    msg,
    {
        btn:[confirm,cancel]
    },
    function () {
        layer.msg(confirmResult, { icon: 1 });
    },
    function () {
        layer.msg(cancelResult, { icon: 2 });
    });
}

/*加载层*/
function Load(number){
    layer.load(number, { shade: false });//0代表加载的风格，支持0-2
    //此处演示关闭
    setTimeout(function () {
        layer.closeAll('loading');
    }, 2000);
}

/*Prompt*/
function Prompt(){
    layer.prompt({
        title: '输入任何口令，并确认',
        formType: 1 //prompt风格，支持0-2
    }, function (pass) {
        layer.prompt({ title: '随便写点啥，并确认', formType: 2 }, function (text) {
            layer.msg('演示完毕！您的口令：' + pass + ' 您最后写下了：' + text);
        });
    });
}




