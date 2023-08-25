let deleteButtons = document.querySelectorAll(".delete-button");

deleteButtons.forEach(deleteButton => {
    deleteButton.onclick = () =>
    {
        if (confirm("Are you sure?"))
        {
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
                    document.getElementById(deleteButton.dataset.id).remove()
                }
                else
                {
                    //some logic
                }        
            })
        }
    }    
});