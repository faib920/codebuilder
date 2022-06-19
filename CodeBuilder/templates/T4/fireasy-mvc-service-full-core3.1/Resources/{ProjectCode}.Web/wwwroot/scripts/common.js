var common = $.extend({}, common);

common.closeWhenSave = true;

common.getJsPath = function (js) {
    var scripts = document.getElementsByTagName("script");
    var path = "";
    for (var i = 0, l = scripts.length; i < l; i++) {
        var src = scripts[i].src;
        if (src.indexOf(js) != -1) {
            path = src.split(js)[0];
            break;
        }
    }
    var href = location.href;
    href = href.split("#")[0];
    href = href.split("?")[0];
    var ss = href.split("/");
    ss.length = ss.length - 1;
    href = ss.join("/");
    if (path.indexOf("https:") == -1 && path.indexOf("http:") == -1 &&
        path.indexOf("file:") == -1 && path.indexOf("\/") != 0) {
        path = href + "/" + path;
    }
    return path;
};

var dialog;
var handler;

//显示对话框
common.showDialog = function (url, title, width, height, hdr, option) {
    var bootPATH = common.getJsPath("common.js");
    var thisPATH = common.getJsPath(location.href);

    var dlg = common.findDialog();
    handler = hdr;
    option = $.extend({ absolutePath: false }, option);
    if (url.substr(0, 1) == '/') {
        option.absolutePath = true;
    }
    option = $.extend({
        id: Math.random(), content: 'url:' + (!option.absolutePath ? thisPATH : '') + url, width: width, height: height, title: title, min: false, max: false, parent: dlg, lock: true, resize: false, wnd: window,
        close: function () {
            //重新置顶，解决无法获得焦点的问题
            if (dlg) dlg.zindex();
            common.closeDialog(dialog.content.window.returnValue);
        }
    }, option);
    dialog = dlg == null ? $.dialog(option) : dlg.opener.$.dialog(option);
    dialog.show();
    return dialog;
}

//查找对话框
common.findDialog = function () {
    if (frameElement != null && frameElement.api != null) {
        return frameElement.api;
    }
    else if (parent.frameElement != null) {
        return parent.frameElement.api;
    }
    return null;
}

//关闭窗口的函数
common.closeDialog = function (result) {
    if (result == null) {
        return;
    }
    if (typeof handler == 'function') {
        if (handler(result)) {
            return;
        }
    }
    //$r.是刷新标识返回值
    if (result == '$r.') {
        common.refreshWindow(dialog.config.wnd);
    }
    else if (typeof result == 'string' && result.indexOf('$r.:') != -1) {
        var dg = result.split(':')[1];
        $('#' + dg).datagrid('reload');
    }
}

//刷新窗口
common.refreshWindow = function (wnd) {
    if (wnd == null) wnd = window;
    if (typeof (wnd.Sys) != 'undefined' && wnd.Sys.WebForms) {
        var mgr = wnd.Sys.WebForms.PageRequestManager.getInstance();
        if (mgr._postBackSettings == null) {
            mgr._postBackSettings = mgr._createPostBackSettings(true, null, null);
        }
        mgr._onFormSubmitHandler();
    }
    else {
        wnd.document.forms[0].submit();
    }
}

//关闭窗口
common.closeWindow = function () {
    var dlg = common.findDialog();
    if (dlg != null) {
        dlg.close();
    }
}

//刷新父页面
common.refreshParent = function (dg) {
    common.findDialog().content.window.returnValue = dg == undefined ? '$r.' : '$r.:' + dg;
}

//设置窗口的返回值
common.setReturnValue = function (value) {
    common.findDialog().content.window.returnValue = value;
}

//显示处理的进度条
common.showProcess = function (title) {
    if (title == null) {
        title = '正在处理，请稍候。。。';
    }
    var mask = $("<div class=\"datagrid-mask\" style=\"display:block;z-index:5000\"></div>");
    var msg = $("<div class=\"datagrid-mask-msg\" style=\"display:block;z-index:5000\"></div>").html(title);
    mask.appendTo($('body'));
    msg.appendTo($('body'));
    msg.css('left', ($('body').width() - msg.width()) / 2);
}

//隐藏处理的进度条
common.hideProcess = function () {
    $('body').children('div.datagrid-mask').remove();
    $('body').children('div.datagrid-mask-msg').remove();
}

//在右下角弹出信息框
common.popup = function (msg) {
    window.top.$.messager.show({
        title: '提示',
        msg: msg,
        timeout: 4000,
        showType: 'slide'
    });
}

//信息信息提示框
common.alert = function (d, fn) {
    if (typeof d == 'object' && typeof (d.succeed) != 'undefined') {
        $.messager.alert('提示', d.msg, d.succeed ? 'info' : 'warning', fn);
    }
    else {
        $.messager.alert('提示', d, 'info', fn);
    }
};

//显示确认对话框
common.confirm = function (msg, fn, f2) {
    $.messager.confirm('提示', msg, function (r) {
        if (r) {
            fn();
        }
        else if (f2) {
            f2();
        }
    });
};

//处理返回值
common.processResult = function (result, succeed, faild, _alert) {
    if (result == null || typeof result == 'undefined') {
        return;
    }

    if (typeof _alert == 'undefined') {
        _alert = true;
    }

    if (result.succeed == false) {
        common.alert(result);
        if (typeof faild == 'function') {
            faild();
        }
    }
    else {
        if (result.succeed == true && result.msg == 'Invalid') {
            $('#form1').form('invalid', result);
        }
        else {
            if (result.succeed == true && _alert) {
                common.popup(result.msg);
            }
            if (typeof succeed == 'function') {
                succeed();
            }
        }
    }
}

common.getUrlParam = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) {
        return unescape(r[2]);
    }

    return null; //返回参数值
};

common.getJsUrlParam = function (js, name) {
    var script = document.getElementsByTagName('script');
    for (var i = 0; i < script.length; i++) {
        me = !!document.querySelector ?
		    script[i].src : script[i].getAttribute('src', 4);

        if (me.substr(me.lastIndexOf('/')).indexOf(js) !== -1) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = me.substr(me.indexOf('?') + 1).match(reg); //匹配目标参数
            if (r != null) {
                return unescape(r[2]);
            }
        }
    }
    return null; //返回参数值
}

//格式化数字
common.formatNumber = function (num, settings1) {
    var regions = [];
    regions[''] = {
        symbol: '',
        positiveFormat: '%s%n',
        negativeFormat: '-%s%n',
        decimalSymbol: '.', //小数点
        digitGroupSymbol: ',', //千分位
        groupDigits: true
    };

    var defaults = {
        name: "formatCurrency",
        colorize: false,
        region: '',
        global: true,
        roundToDecimalPlace: 2, //小数位
        eventOnDecimalsEntered: false
    };

    //defaults = $.extend(defaults, regions['']);
    var settings = $.extend({
        name: "formatCurrency",
        colorize: false,
        region: '',
        global: true,
        roundToDecimalPlace: 2, //小数位
        eventOnDecimalsEntered: false, symbol: '',
        positiveFormat: '%s%n',
        negativeFormat: '-%s%n',
        decimalSymbol: '.', //小数点
        digitGroupSymbol: ',', //千分位
        groupDigits: true
    }, settings1);

    num = '' + num;
    if (num.search('\\(') >= 0) {
        num = '-' + num;
    }

    if (num === '' || (num === '-' && settings.roundToDecimalPlace === -1)) {
        return;
    }

    // if the number is valid use it, otherwise clean it
    if (isNaN(num)) {
        // clean number
        num = num.replace(settings.regex, '');

        if (num === '' || (num === '-' && settings.roundToDecimalPlace === -1)) {
            return;
        }

        if (settings.decimalSymbol != '.') {
            num = num.replace(settings.decimalSymbol, '.');  // reset to US decimal for arithmetic
        }
        if (isNaN(num)) {
            num = '0';
        }
    }

    // evalutate number input
    var numParts = String(num).split('.');
    var isPositive = (num == Math.abs(num));
    var hasDecimals = (numParts.length > 1);
    var decimals = (hasDecimals ? numParts[1].toString() : '0');
    var originalDecimals = decimals;

    // format number
    num = Math.abs(numParts[0]);
    num = isNaN(num) ? 0 : num;
    if (settings.roundToDecimalPlace >= 0) {
        decimals = parseFloat('1.' + decimals); // prepend "0."; (IE does NOT round 0.50.toFixed(0) up, but (1+0.50).toFixed(0)-1
        decimals = decimals.toFixed(settings.roundToDecimalPlace); // round
        if (decimals.substring(0, 1) == '2') {
            num = Number(num) + 1;
        }
        decimals = decimals.substring(2); // remove "0."
    }
    num = String(num);

    if (settings.groupDigits) {
        for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++) {
            num = num.substring(0, num.length - (4 * i + 3)) + settings.digitGroupSymbol + num.substring(num.length - (4 * i + 3));
        }
    }

    if ((hasDecimals && settings.roundToDecimalPlace == -1) || settings.roundToDecimalPlace > 0) {
        num += settings.decimalSymbol + decimals;
    }

    // format symbol/negative
    var format = isPositive ? settings.positiveFormat : settings.negativeFormat;
    var money = format.replace(/%s/g, settings.symbol);
    money = money.replace(/%n/g, num);

    return money;
}

//向post data里添加令牌以防止CSRF攻击
common.antiToken = function (data) {
    var o = $('input[name="__RequestVerificationToken"]');
    if (o.length == 1) {
        data["__RequestVerificationToken"] = o.val();
    }
}

/* 禁止空格键 */
function banBackspace(e) {
    var ev = e || window.event; //获取event对象
    var obj = ev.target || ev.srcElement; //获取事件源
    var t = obj.type || obj.getAttribute('type'); //获取事件源类型
    //获取作为判断条件的事件类型
    var vReadOnly = obj.readOnly;
    var vDisabled = obj.disabled;
    //处理undefined值情况
    vReadOnly = (vReadOnly == undefined) ? false : vReadOnly;
    vDisabled = (vDisabled == undefined) ? true : vDisabled;
    //当敲Backspace键时，事件源类型为密码或单行、多行文本的，
    //并且readOnly属性为true或disabled属性为true的，则退格键失效
    var flag1 = ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea") && (vReadOnly == true || vDisabled == true);
    //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效
    var flag2 = ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea";
    //判断
    if (flag2 || flag1) return false
}

//禁止退格键 作用于Firefox、Opera
document.onkeypress = banBackspace;
//禁止退格键 作用于IE、Chrome
document.onkeydown = banBackspace;

$(function () {
    //注册按下回车后进行查询
    $('.enterQuery').each(function (i, r) {
        var _this = $(r);
        var e = _this;
        if (_this.hasClass('easyui-textbox')) {
            e = _this.next().find('input.textbox-text');
        }

        e.keydown(function (event) {
            if (event.which == 13) {
                event.preventDefault();
                if (typeof query == 'function') {
                    if (e != _this) {
                        _this.textbox('setValue', $(this).val());
                    }
                    query();
                }
            }
        });
    });
});

$.ajaxSetup({
    complete: function () {
        if ($('div.datagrid-mask').length > 0) {
            common.hideProcess();
        }
    },
    beforeSend: function (xhr) {
        xhr.setRequestHeader('Pragma', 'no-cache');
    },
    error: function () {
        alert(arguments[0].responseText);
    }
});
