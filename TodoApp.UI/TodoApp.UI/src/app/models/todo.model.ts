export interface Todo{
    id:string,
    description:string,
    isCompleted:boolean,
    createdDate:Date,
    completedDate?:Date,
    isDeleted: boolean,
    deletedDate: Date
}