


ExportData = function (d, c) {
    var workbook = ExcelBuilder.Builder.createWorkbook();
    var worksheet = workbook.createWorksheet({ name: 'Sheet1' });
    var stylesheet = workbook.getStyleSheet();

    var originalData = d

    var albumTable = new ExcelBuilder.Table();
    albumTable.setReferenceRange([1, 1], [c.length, originalData.length]);

    var col = [];
    $.each(c, function (i, val) {
        col.push(val.name);
    })

    var dat = [];
    dat.push(col);
    $.each(d, function (i, val) {
        var data = [];
        $.each(c, function (i2, val2) {
            data.push(val[val2.id]);
        })
        dat.push(data);
    })

    var wid = [];
    $.each(c, function (i, val) {
        wid.push({ width: (val.width > 50 ? (val.width / 10) : val.width) });
    })

    worksheet.setData(dat);

    worksheet.setColumns(wid);

    workbook.addWorksheet(worksheet);

    albumTable.setTableColumns(col);
    worksheet.addTable(albumTable);
    workbook.addTable(albumTable);

    ExcelBuilder.Builder.createFile(workbook).then(function (data) {
        window.open("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + data, '_blank');
    });
}