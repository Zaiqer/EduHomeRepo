let skipCourse = 6;
let courseCount = $("#loadMore").next().val();
$(document).on("click", "#loadMore", () => {
    $.ajax({
        url: "/Courses/LoadMore/",
        type: "post",
        data: {
            "skip": skipCourse
        },
        success: function (res) {
            $("#myCourses").append(res)
            skipCourse += 6;
            if (courseCount <= skipCourse) {
                $("#loadMore").remove()
            }
        }
    })
});