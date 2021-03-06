var app = angular.module('app', ['ngTouch', 'ui.grid', 'ui.grid.resizeColumns', 'ui.grid.grouping', 'ui.grid.moveColumns',
    'ui.grid.selection', 'ui.grid.exporter', 'ui.grid.cellNav', 'ui.grid.pinning']);

app.controller('MainCtrl', ['$scope', '$log', '$http', 'uiGridConstants', 'uiGridGroupingConstants',

  function ($scope, $log, $http, uiGridConstants) {

      
      //$scope.gridOptions = {};


      $scope.gridOptions = {
          enableHorizontalScrollbar: uiGridConstants.scrollbars.ALWAYS,
          enableFiltering: true,
          showColumnFooter: true,
          enableGridMenu: true,
          enableSelectAll: true,
          exporterCsvFilename: 'myFile.csv',
          exporterPdfDefaultStyle: { fontSize: 9 },
          exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
          exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
          exporterPdfHeader: { text: "My Header", style: 'headerStyle' },
          exporterPdfFooter: function (currentPage, pageCount) {
              return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
          },
          exporterPdfCustomFormatter: function (docDefinition) {
              docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
              docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
              return docDefinition;
          },
          exporterPdfOrientation: 'portrait',
          exporterPdfPageSize: 'LETTER',
          exporterPdfMaxGridWidth: 500,
          

          exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
          onRegisterApi: function (gridApi) {
              $scope.gridApi = gridApi;
          }
      };
      


      function GetColumnWidth(results,j) {
          var ColWidth = 130;
          if (results.Data[0][j]["Value"] != null) {
              if ((results.Data[0][j]["Value"].length * 10).toString() != "NaN") {
                  ColWidth = results.Data[0][j]["Value"].length * 10;
              }
              else {
                  ColWidth = results.Data[0][j]["Key"].length * 10
              }
          }
          else {
              ColWidth = results.Data[0][j]["Key"].length * 10
          }

          if (ColWidth < 130)
              ColWidth = 130;

          return ColWidth;
      }


      $scope.BindData = function ()
      {
          $.ajax({
              url: '/GridReport/GridReportFill/' + $(this).serialize(),
              type: "POST",
              data: $("#registerSubmit").serialize(),
              success: function (result) {
                  Lock = false;
                  if (result.Success == true) {
                      var results = result;
                      if (results.Data.length > 0) {
                          var columnsIn = results.Data[results.Data.length - 1];
                          var j = 0;
                          var ColumnCount = 0;

                          $scope.gridOptions.columnDefs = new Array();

                          $.each(columnsIn, function (key, value) {
                              if (columnsIn[j]["Key"] != "SysParamType") {
                                  var ColWidth = GetColumnWidth(results, j);

                                  $scope.gridOptions.columnDefs.push({
                                      field: columnsIn[j]["Key"], aggregationType: columnsIn[j]["Value"],
                                      cellClass: (columnsIn[j]["Value"] == null ? 'cell-text' : 'text-right cell-text'),
                                      aggregationHideLabel: true,
                                      headerCellClass: (columnsIn[j]["Value"] == null ? 'header-text' : 'text-right header-text'),
                                      footerCellClass: (columnsIn[j]["Value"] == null ? '' : 'text-right '),
                                      width: ColWidth,
                                      enablePinning: true,
                                  });
                                }
                              ColumnCount++;
                              j++;
                              
                              
                          });


                          var rowDataSet = [];
                          var i = 0;
                          $.each(results.Data, function (key, value) {
                                var rowData = [];
                                var j = 0   
                                var columnsIn = results.Data[i];
                                if (columnsIn[ColumnCount - 1]["Value"] == null)
                                {
                                    $.each(columnsIn, function (key, value) {
                                        rowData[columnsIn[j]["Key"]] = columnsIn[j]["Value"];
                                        //alert(columnsIn[j]["Key"]);
                                        j++;
                                    });
                                }
                                rowDataSet[i] = rowData;
                                i++;
                          });

                          $scope.gridOptions.data = rowDataSet;

                          $scope.gridApi.grid.refresh();

                      }
                  }
                  else if (!result.Success) {
                      alert('Something went wrong');
                  }
              },
              error: function () {
                  Lock: false;
                  alert('Something went wrong');
              }
          });

          
      }

  }
]);






