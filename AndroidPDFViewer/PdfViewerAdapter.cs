using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidPDFViewer
{
    public class PdfViewerAdapter : BaseAdapter<Bitmap>
    {

        Context context;
        private List<Bitmap> _bitmaps;
        public PdfViewerAdapter(Context context, List<Bitmap> bitmaps)
        {
            this.context = context;
            _bitmaps = bitmaps;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            view = inflater.Inflate(Resource.Layout.pdf_listview_cell, parent, false);
            var imageView = view.FindViewById<ImageView>(Resource.Id.pdfPageImage);
            imageView.SetImageBitmap(_bitmaps[position]);
            return view;
        }

        public override int Count => _bitmaps.Count();


        public override Bitmap this[int position] => _bitmaps[position];
    }

    class PdfViewerAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}