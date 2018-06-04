
//window.onload = function () {
//    var model =  
//    console.log(typeof model);
//    console.log(model.ClickCount);
//    var total = Number(document.Model.ClickCount) + Number(document.Model.ProcessedCount) + Number(document.Model.DeferredCount) + Number(document.Model.DeliveredCount) + Number(document.Model.OpenCount);
//    var Processed = Number(document.Model.ProcessedCount) / total;
//    console.log(typeof total);
//    console.log(total);
//    console.log(Processed);
//    var Deliverred = document.ModelDeliveredCount / total;
//    var Deferred = document.ModelDeferredCount / total;
//    var Open = document.ModelOpenCount / total;
//    var Click = document.ModelClickCount / total;
//    var chart = new CanvasJS.Chart("chartContainer", {
//        animationEnabled: true,
//        theme: "light2",
//        title: {
//            text: "Email"
//        },
//        tooltipFillColor: "rgba(51, 51, 51, 0.55)",
//        data: [{
//            type: "pie",
//            indexLabelFontSize: 18,
//            radius: 80,
//            indexLabel: "{label} - {y}",
//            yValueFormatString: "###0.0\"%\"",
//            click: explodePie,
//            dataPoints: [
//                { y: Processed, label: "Processed" },
//                { y: Deliverred, label: "Deliverred" },
//                { y: Deferred, label: "Deferred" },
//                { y: Open, label: "Open" },
//                { y: Click, label: "Click" }
//            ],
//            backgroundColor: [
//                "#BDC3C7",
//                "#9B59B6",
//                "#E74C3C",
//                "#26B99A",
//                "#3498DB"
//            ],
//            hoverBackgroundColor: [
//                "#CFD4D8",
//                "#B370CF",
//                "#E95E4F",
//                "#36CAAB",
//                "#49A9EA"
//            ]
//        }]
//    });
//    chart.render();

//    function explodePie(e) {
//        for (var i = 0; i < e.dataSeries.dataPoints.length; i++) {
//            if (i !== e.dataPointIndex)
//                e.dataSeries.dataPoints[i].exploded = false;
//        }
//    }

//};