let taskNameInput = document.querySelector(".new-task-name")
let taskDescriptionInput = document.querySelector(".new-task-description")
let addTaskBtn = document.querySelector(".add-new-task-submit-btn")

let deleteButtons = document.querySelectorAll(".task-delete-btn")
deleteButtons.forEach(deleteButton => {
    deleteButton.addEventListener("click", DeleteTaskById.bind(deleteButton, deleteButton.dataset.id))
});

let checkBoxes = document.querySelectorAll(".task-checkbox")
checkBoxes.forEach(checkBox => {
    checkBox.addEventListener("change", UpdateTaskStatus.bind(checkBox, checkBox.dataset.id))
});

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
        AppendTask(result)
    })
}

function AppendTask(task)
{
    let taskStatus = document.createElement("div");
    taskStatus.className = "task-status"
    let taskCheckbox = document.createElement("input");
    taskCheckbox.type = "checkbox";
    taskCheckbox.className = "task-checkbox";
    taskCheckbox.dataset.id = task.id
    taskCheckbox.addEventListener("change", UpdateTaskStatus.bind(taskCheckbox, task.id))
    taskStatus.appendChild(taskCheckbox);
    
    let taskInfo = document.createElement("div");

    taskInfo.className = "task-info";
    let taskNameDescription = document.createElement("div");
    taskNameDescription.className = "task-name-description";
    let taskName = document.createElement("div");
    taskName.className = "task-name";
    taskName.innerHTML = task.name;
    let taskDescription = document.createElement("div");
    taskDescription.className = "task-description";
    taskDescription.innerHTML = task.description;
    taskNameDescription.appendChild(taskName);
    taskNameDescription.appendChild(taskDescription);

    let taskStartEnd = document.createElement("div");
    taskStartEnd.className = "task-start-end";
    let taskCreationDate = document.createElement("div");
    taskCreationDate.className = "task-creation-date";
    taskCreationDate.innerHTML = `Start at: ${DateFormat(new Date(task.creationDateTime))}`;
    let taskCompletionDate = document.createElement("div");
    taskCompletionDate.className = "task-completion-date";
    taskCompletionDate.id = `${task.id}-completion-date`
    taskCompletionDate.innerHTML = `End at: `;
    taskStartEnd.appendChild(taskCreationDate);
    taskStartEnd.appendChild(taskCompletionDate);

    let taskDeleteBtnWrapper = document.createElement("div");
    taskDeleteBtnWrapper.className = "task-delete-btn-wrapper";
    let taskDeleteBtn = document.createElement("button");
    taskDeleteBtn.className = "task-delete-btn";
    taskDeleteBtn.dataset.id = task.id;
    taskDeleteBtn.innerHTML = "Delete"
    taskDeleteBtnWrapper.appendChild(taskDeleteBtn);
    
    taskInfo.appendChild(taskNameDescription);
    taskInfo.appendChild(taskStartEnd);
    taskInfo.appendChild(taskDeleteBtnWrapper);

    let taskWrapper = document.createElement("div");
    taskWrapper.className = "task-wrapper task-in-progress";
    taskWrapper.id = task.id;
    taskWrapper.appendChild(taskStatus);
    taskWrapper.appendChild(taskInfo);

    document.querySelector(".asd").insertBefore(taskWrapper, document.querySelector(".add-new-task-wrapper"))

    taskDeleteBtn.addEventListener("click", DeleteTaskById.bind(taskDeleteBtn, task.id))
}

function DeleteTaskById(id)
{
    if (!confirm("Are you sure?"))
    {
        return;
    }

    let body = 
    {
        taskId: id
    }

    fetch(`${window.location.origin}/api/lists/tasks`, {
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
            document.getElementById(id).remove()
        }
        else
        {
            //some logic
        }        
    })
}

function UpdateTaskStatus(id)
{
    let body =
    {
        taskId: id
    }

    fetch(`${window.location.origin}/api/lists/tasks`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
    })
    .then((response) => {
        if (response.status == 200)
        {
            return response.json();
        }
        else
        {
            //some logic
        }        
    })
    .then((result) => {
        if (result.completionDateTime == null)
        {
            document.getElementById(`${id}-completion-date`).innerHTML = `End at: `
            document.getElementById(id).className = "task-wrapper task-in-progress"
        }
        else
        {
            document.getElementById(`${id}-completion-date`).innerHTML = `End at: ${DateFormat(new Date(result.completionDateTime))}`
            document.getElementById(id).className = "task-wrapper task-is-completed"
        }
        
    })
}

function DateFormat(date)
{
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const year = date.getFullYear();
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');

    return `${month}.${day}.${year} ${hours}:${minutes}`;
}