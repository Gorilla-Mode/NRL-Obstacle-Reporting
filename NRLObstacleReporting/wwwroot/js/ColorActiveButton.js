/**
 * add "but" in html class to invoke. Changes button color when active
 * @param defaultcolor CSS color of an inactive button
 * @param activecolor CSS color of active button
 */
function ColorActiveButton(defaultcolor, activecolor) 
{
    const defaultColor = defaultcolor; 
    const activeColor = activecolor; 
    
    const buttons = document.querySelectorAll('.but'); //Puts all buttons in list
   
    //Adds click listener to each button
    buttons.forEach((but) => 
    {
        but.addEventListener('click', () => 
        {
            //updates all buttons to default color
            buttons.forEach((b) => 
            {
                b.classList.add(defaultColor);
                b.classList.remove(activeColor);
            });
            
            //updates active button to active color
            but.classList.remove(defaultColor);
            but.classList.add(activeColor);
        });
    });
}