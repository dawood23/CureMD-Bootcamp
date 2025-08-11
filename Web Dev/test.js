function getData() {

    $.ajax({
        url: 'https://jsonplaceholder.typicode.com/todos',
        method: 'GET',
        dataType: 'json',
        success: function(data) {
            displayData(data);
        },
        error: function() {
            console.log("error occurred");
        }
    });
}

function displayData(data) {
    var container = $('.container');
    container.empty();

    $.each(data, function(index, todo) {
    
       const card = `
        <div class="card">
            <h3>${todo.title}</h3>
            <p><strong>ID:</strong> ${todo.id}</p>
            <p><strong>User ID:</strong> ${todo.userId}</p>
            <p><strong>Status:</strong> ${todo.completed ? '✅ Completed' : '❌ Not Completed'}</p>
            <div class="actions">
                <button class="update-btn" data-id="${todo.id}">Update</button>
                <button class="delete-btn" data-id="${todo.id}">Delete</button>
            </div>
        </div>
    `;
        container.append(card);
    });

    $('.update-btn').click(function() {
        const id = $(this).data('id');
        console.log("Update clicked for ID:", id);
    });

    $('.delete-btn').click(function() {
        const id = $(this).data('id');
        deletedata(id);
    });
}

function AddData(data){
    $.ajax({
        url:"https://jsonplaceholder.typicode.com/todos",
        method:"POST",
        data:JSON.stringify({
            title:data.title,
            status:data.completed=="completed"
        }),
        success:function(){
            console.log("data added")
            getData();
        },
        error: function(){
            console.log("Error in creating the item");
        }
    })
}

function deletedata(id){
    $.ajax({
        url:"https://jsonplaceholder.typicode.com/todos/"+id,
        method:"DELETE",
        success:function(){
            console.log("Data deleted for id: "+id);
            getData();
        },
        error:function(){
            console.log("An error occured while deleting the data")
        }
    })
}

$(document).ready(function() {
    console.log("Application Loaded");

    const adbtn = document.getElementsByClassName('Add-Button')[0];
    const addform = document.getElementsByClassName('add-form-container')[0];

    adbtn.addEventListener('click', () => {
        addform.style.display = 'block';
    });

    const confirmadd=document.getElementById('confirm-add')
    confirmadd.addEventListener('click',(event)=>{
        event.preventDefault();

        const title=document.getElementById('title').value;
        const status=document.querySelector('input[name="status"]:checked')?.value;
        if(title==''){
            alert("Title cannot be null")
            addform.style.display='none'
            return
        }
        AddData({title:title,status:status})
        

        document.getElementById('title').value = '';
        document.querySelectorAll('input[name="status"]').forEach((s) => {
        s.checked = false;
        });
    
        addform.style.display='none'
    })

    getData();
});

