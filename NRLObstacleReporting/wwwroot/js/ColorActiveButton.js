//changes color of active button
//input css class as parameter
//add "but" in css class to target
function ColorActiveButton(defaultcolor, activecolor) 
{
    const defaultColor = defaultcolor; 
    const activeColor = activecolor; 
    
    const buttons = document.querySelectorAll('.but');
   
    buttons.forEach((but) => 
    {
        but.addEventListener('click', () => 
        {
            buttons.forEach((b) => 
            {
                b.classList.add(defaultColor);
                b.classList.remove(activeColor);
            });
            
            but.classList.remove(defaultColor);
            but.classList.add(activeColor);
        });
    });
}