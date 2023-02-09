namespace AeroVendas.ULF.Cliente.Pages;

    public partial class EditorWidget
    {
        private string content = "";
        bool disable = false;
        private Dictionary<string, object> editorConf = new Dictionary<string, object>{
        {"toolbar", "save | insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
        },
        {"plugins",  "['advlist autolink lists link image charmap print preview anchor,save,autosave, " +
        "'searchreplace visualblocks code fullscreen'," +
        "'insertdatetime media table paste code help wordcount']"
        },
        {"content_style",  "body { font-family:Helvetica,Arial,sans-serif; font-size:14px" }
        //{"skin", "oxide-dark" }
        ////{"content_css", "dark" }

        };
    }

