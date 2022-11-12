$(document).on("keyup", "#search-courses", () => {
    $("#myCourses").empty()
    $.ajax({
        url: "/Courses/CoursesSearch/",
        type: "get",
        data: {
            "key": $("#search-courses").val()
        },
        success: function (res) {
            $("#myCourses").append(res)
        }
    })
});