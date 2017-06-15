
namespace mobile_style_editor
{
    public class TitleEditButton : ImageButton
    {
        public TitleEditButton()
        {
            Source = "icon_edit.png";

            double padding = 3;

#if __IOS__
            padding = 8;
#endif
            ImagePadding = padding;
            LeftPadding = 1;
            TopPadding = 2;
        }
    }
}
