/**
 * Searches an entire rows for any matches from where tableid is set.
 * @param TableId Id of the table to search
 * @param SeachInput id of the input where a seach is inputted
 */
function searchTable(TableId, SeachInput)
{
    var $rows = $(`#${TableId} tr`);
    $(`#${SeachInput}`).keyup(function() 
    {
        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();

        $rows.show().filter(function() 
        {
            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
            return !~text.indexOf(val);
        }).hide();
    });
}