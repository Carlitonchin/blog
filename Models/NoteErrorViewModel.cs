namespace blog.Models;

public class NoteErrorViewModel{
    public int StatusCode {get; set;}
    public string Message {get;set;}
    public string Action {get; set;}

    public NoteErrorViewModel(int status, string message, string action){
        this.StatusCode = status;
        this.Message = message;
        this.Action = action;
    }
}