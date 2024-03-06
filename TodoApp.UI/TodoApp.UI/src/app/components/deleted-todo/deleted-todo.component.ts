import { Component, OnInit } from '@angular/core';
import { Todo } from '../../models/todo.model';
import { TodoService } from '../../services/todo.service';
import { response } from 'express';

@Component({
  selector: 'app-deleted-todo',
  templateUrl: './deleted-todo.component.html',
  styleUrl: './deleted-todo.component.css'
})
export class DeletedTodoComponent implements OnInit{
  todos:Todo[] = [];

  constructor(private todoServie:TodoService){}

  ngOnInit(): void {
    this.getAllDeletedTodos();
  }

  getAllDeletedTodos(){
    this.todoServie.getAllDeletedTodos().subscribe({
      next: (response) => {
        this.todos = response;
      }
    })
  }

  undoDelete(id:string, todo:Todo){
    this.todoServie.undoDelete(id,todo).subscribe({
      next: (response) =>{
        this.getAllDeletedTodos();
      }
    })
  }

}
