$("#submitBirthdate").on("click", () => {
    let birthdateData = {
        "birthMonth": parseInt($("#birthMonth").val()),
        "birthDay": parseInt($("#birthDay").val())
    };
    $.ajax({
        type: "POST",
        url: "Home/GetNextDatesSource",
        data: JSON.stringify({ birthdateSerialized: JSON.stringify(birthdateData) }),
        contentType: "application/json; charset=utf-8",
        success: (response) => {
            if (response.error != null) {
                alert(response.error);
            }
            else {
                let resultHtml = "";
                $.each(response, (index, presentsDay) => {
                    resultHtml += "<h4>" + presentsDay.Name + "</h4><p>Presents in " + presentsDay.DaysLeft + " days</p>";
                });
                $("#result").html(resultHtml);
            }
        }
    });
});