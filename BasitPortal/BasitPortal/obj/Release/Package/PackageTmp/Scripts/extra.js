
function CheckedChanged() {
    console.log("checkedchanged içindesin");
     if ((document.getElementById("CheckBox1").checked)){
       GetTotalQuantity();
   }
   else{
       GetTotalQuantity();
   }
}
function GetTotalQuantity() {
    console.log("gettotal içindesin");
    var total = document.getElementById("lblQantity");
    var grid = $find("<%=gvInventory.ClientID %>");
    var MasterTable = grid.get_masterTableView();
    var selectedRows = MasterTable.get_selectedItems();
    for (var i = 0; i < selectedRows.length; i++) {
        var row = selectedRows[i];
        if (MasterTable.getElementById("CheckBox1").checked) {
            var cell = MasterTable.getCellByColumnUniqueName(row, "untqty");
            console.log(cell);
            total += cell;
        }  
    }
}
