let toDoListCards = document.querySelectorAll(".to-to-list-card");
toDoListCards.forEach(toDoListCard => {
    toDoListCard.onclick = () => { window.location = `${window.location.origin}/list/listtasks/${toDoListCard.id}` }
});

let deleteButtons = document.querySelectorAll(".delete-button");

deleteButtons.forEach(deleteButton => {
    deleteButton.onclick = (event) =>
    {
        event.stopPropagation()

        if (!confirm("Are you sure?"))
        {
            return;
        }

        let body = 
        {
            listId: deleteButton.dataset.id
        }

        fetch(`${window.location.origin}/api/lists`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        })
        .then((response) => {
            if (response.ok)
            {
                document.getElementById(`${deleteButton.dataset.id}`).remove()
            }
            else
            {
                //some logic
            }        
        })
    }    
});