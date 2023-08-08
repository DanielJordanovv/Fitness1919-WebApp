$(function () {
    $('.calorieForm').submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: '@Url.Action("Calculate", "CalorieCalculator")',
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                $('#result').html(result);
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                alert('An error occurred while calculating the calories. Please try again.');
            }
        });
    });
});