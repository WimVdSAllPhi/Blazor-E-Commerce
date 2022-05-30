export function updateCurvedText(curvedText) {
    var demo4 = new CircleType(curvedText);
    demo4.radius(demo4.element.offsetWidth / 2);

    $(demo4.element).fitText();

    var resizeMain = $('.resizeMain');

    if (resizeMain.length) {
        resizeMain.height($(demo4.element).height() + 50);
    }

    window.addEventListener('resize', function updateRadius() {
        /*demo4.radius(demo4.element.offsetWidth / 2);*/
        var resizeMain = $('.resizeMain');

        if (resizeMain.length) {
            resizeMain.height($(demo4.element).height());
        }
    });

    return {
        dispose: () => {
        }
    }
}

//$(document).ready(function () {
//    var $curvedText = $(".curved-text");
//    updateCurvedText($curvedText, 880);
//});
