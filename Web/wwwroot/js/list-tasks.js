let taskNameInput = document.querySelector(".new-task-name")
let taskDescriptionInput = document.querySelector(".new-task-description")
let addTaskBtn = document.querySelector(".add-new-task-submit-btn")

addTaskBtn.onclick = () =>
{
    let newTask =
    {
        listId: window.location.href.split("/").pop(),
        taskName: taskNameInput.value,
        taskDescription: taskDescriptionInput.value
    }

    fetch(`${window.location.origin}/api/lists/tasks`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newTask)
    })
    .then((response) => {
        if (response.status == 200)
        {
            taskNameInput.value = "";
            taskDescriptionInput.value = "";
            return response.json();
        }
        else
        {
            //some logic
        }        
    })
    .then((result) => {
    })
}