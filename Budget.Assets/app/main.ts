import * as $ from "jquery";
import * as bootstrap from "bootstrap";
import "../bower_components/jquery-validation/dist/jquery.validate";
import "../bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive";

$("table#allowanceDays td").on("click", (evt) => {
    var data = $(evt.target).data();
    $.post(`/AllowanceTasks/AddDay`, data, (response: string[]) => {
        let idx = response.indexOf(data.day);
        if (idx >= 0) {
            $(evt.target).text("X");
        }
        console.log(response);
    })
})