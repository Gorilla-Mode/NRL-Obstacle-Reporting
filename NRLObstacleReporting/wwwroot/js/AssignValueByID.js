//for your own sanity make sure the input into the function is absolutely correct, shit made me lose my mind with 
//useless fucking type errors, yeah, thanks for letting me know a string is undefined "GET REAL" - David Lynch, 2008 
function AssignValueByID (ButtonID, InputID, Value){
    document.getElementById(ButtonID).addEventListener("click", function() {
        document.getElementById(InputID).value = Value;
    });
}