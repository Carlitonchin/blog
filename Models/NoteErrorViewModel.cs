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

    private static NoteErrorViewModel NewNoteErrorViewModel(int status, string action, int note_id){
        string message = "";

        if(status == CodeError.NotFound)
            message = "La nota con id = '" + note_id + "' no existe";
        else if(status == CodeError.NotFound)
            message = "Usted no es el autor de la nota con id ='" + note_id + "'";
        else
            message = "Ocurrió un error inesperado, por favor contacte con el administrador";

        return new NoteErrorViewModel(status, message, action);
    }

    public static NoteErrorViewModel Internal(string action, int note_id){
        return NewNoteErrorViewModel(CodeError.Internal, action, note_id);
    }

    public static NoteErrorViewModel NotFound(string action, int note_id){
        return NewNoteErrorViewModel(CodeError.NotFound, action, note_id);
    }

    public static NoteErrorViewModel NotAuthorized(string action, int note_id){
        return NewNoteErrorViewModel(CodeError.NotAuthorized, action, note_id);
    }
}

 static class CodeError{
    public const int NotFound = 404;
    public const int NotAuthorized = 401;
    public const int Internal = 500;
}