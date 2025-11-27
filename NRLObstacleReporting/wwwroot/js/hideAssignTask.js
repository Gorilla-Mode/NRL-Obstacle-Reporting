/**
 * Hides a div when called
 */
function HideAssignTask() 
{
    var x = document.getElementById(`AssignTask`);
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}