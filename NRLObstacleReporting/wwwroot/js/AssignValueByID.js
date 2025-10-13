//for your own sanity make sure the input into the function is absolutely correct, shit made me lose my mind with 
//useless fucking type errors, yeah, thanks for letting me know a string is undefined "GET REAL" - David Lynch, 2008 

/**
 * Assigns a value to an input field when a specified button is pressed. CASE SENSITIVE!
 * @param ButtonID ID of the button to run the function
 * @param InputID The ID of the field that value will be assigned to
 * @param Value The value to be assigned to ButtonID
 */
function AssignValueByID (ButtonID, InputID, Value) 
{
    //adds click listener to button
    document.getElementById(ButtonID).addEventListener("click", function() 
    {
        //on click, updates field value
        document.getElementById(InputID).value = Value;
    });
}