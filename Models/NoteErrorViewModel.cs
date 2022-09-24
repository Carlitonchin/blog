namespace blog.Models;

public class NoteErrorViewModel{
    public int StatusCode {get; set;}
    public string Message {get;set;}

    public NoteErrorViewModel(int status, string message){
        this.StatusCode = status;
        this.Message = message;
    }

    private static NoteErrorViewModel NewNoteErrorViewModel(int status, int note_id){
        string message = "";

        if(status == CodeError.NotFound)
            message = "La nota con id = '" + note_id + "' no existe";
        else if(status == CodeError.NotAuthorized)
            message = "Usted no es el autor de la nota con id ='" + note_id + "'";
        else if(status == CodeError.BadRequest)
            message = "Error en el request, intente navegar con los links de la página";
        else
            message = "Ocurrió un error inesperado, por favor contacte con el administrador";

        return new NoteErrorViewModel(status, message);
    }

    public static NoteErrorViewModel Internal(){
        return NewNoteErrorViewModel(CodeError.Internal, -1);
    }

    public static NoteErrorViewModel NotFound(int note_id){
        return NewNoteErrorViewModel(CodeError.NotFound, note_id);
    }

    public static NoteErrorViewModel NotAuthorized(int note_id){
        return NewNoteErrorViewModel(CodeError.NotAuthorized, note_id);
    }

    public static NoteErrorViewModel BadRequest(){
        return NewNoteErrorViewModel(CodeError.BadRequest, -1);
    }
}

 public static class CodeError{
    public const int NotFound = 404;
    public const int NotAuthorized = 401;
    public const int Internal = 500;
    public const int BadRequest = 400;
}