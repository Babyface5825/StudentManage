
function HTMLEnCode(str) {
    var s = JSON.stringify(str);    
    if (s.length == 0) return "";
    //s = s.replace(/&/g, "&gt;");    
    s = s.replace(/</g, "&lt;");    
    s = s.replace(/>/g, "&gt;");
    //s = s.replace(/ /g, "&nbsp;");
    s = s.replace(/\'/g, "&39;");
    //s = s.replace(/\"/g, "&quot;");
    s = s.replace(/\n/g, "");
    s = s.replace(/\r/g, "");
    s = s.replace(/\t/g, "");
    return s;
}

function HTMLEnCodeQuot(str) {
    var s = str;
    if (s.length == 0) return "";
    s = s.replace(/\\\"/g, "&quot;");
    return s;
}


function HTMLDeCode(str) {
    var s = "";
    if (str.length == 0) return "";
    //s = str.replace(/&gt;/g, "&");
    s = str.replace(/&lt;/g, "<");
    s = s.replace(/&gt;/g, ">");
    s = s.replace(/&nbsp;/g, " ");
    s = s.replace(/&39;/g, "'");
    s = s.replace(/&quot;/g, "\"");
    //s = s.replace(/<br>/g, "\n");
    return s;
}

var getIds = function (grid) {
    var sel = Ext.getCmp("ga").getSelectionModel().getSelections();
    //var sel = grid.getSelectionSubmit().getSelectionModelField().getValue();
    var ids = "";
    if (sel.length > 0) {
        for (var i = 0; i < sel.length - 1; i++) {
            ids = ids + sel[i].get("ID") + ",";
        }
        ids = ids + sel[sel.length - 1].get("ID");
    }
    else {
        Ext.Msg.alert("确认选择", "至少选择一行，请确认！");
    }
    return ids;
}

var getGridValueArr = function (grid, mem) {
    var grid_data = grid.getStore();
    var count = grid_data.getCount();
    var arr = new Array(count);
    for (var i = 0 ; i < count ; i++) {
        arr[i] = grid_data.getAt(i).data[mem];
    }
    return arr;
};