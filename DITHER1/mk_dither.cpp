#include "stdafx.h"
#include "dither.h"
#include "ditherDlg.h"
#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
//=============================================================================
void
CDitherDlg::OnRun()
{
  int R0 = GetDlgItemInt( IDC_RANGE0_EDIT );
  int R1 = GetDlgItemInt( IDC_RANGE1_EDIT );

  if( R0 < 1 || R0 > 8 )
  {
    AfxMessageBox( "Недопустимое значение Range 0", MB_ICONERROR );
    return;
  }

  if( R1 < R0 || R1 > 16 )
  {
    AfxMessageBox( "Недопустимое значение Range 1", MB_ICONERROR );
    return;
  }

  int t = R1 - R0;
  if( R0 > 1 && ( t & 1 ))
  {
    AfxMessageBox( "Недопустимое сочетание Range 0 и Range 1", MB_ICONERROR );
    return;
  }
  GetDlgItem( IDC_RUN )->EnableWindow( 0 );
  ExampleBox.Draw( R0, R1, IsDlgButtonChecked( IDC_DITHER_CHECK ));
  GetDlgItem( IDC_RUN )->EnableWindow( 1 );
}
//=============================================================================
static int*
MkMtx( int r )
{
  if( r < 2 )
    return 0;

  static int const m2[2][2] = {{ 0, 3 }, { 2, 1 }};

  int* m = new int[r*r];
  if( r == 2 )
    memcpy( m, m2, sizeof m2 );
  else
  {
    int k = r / 2;
    int* l = MkMtx( k );
    for( int i = k ; --i >= 0 ; )
      for( int j = k ; --j >= 0 ; )
      {
        int v = l[i*k+j]*4;
        for( int i1 = 2 ; --i1 >= 0 ; )
          for( int j1 = 2 ; --j1 >= 0 ; )
            m[(i+k*i1)*r+(j+k*j1)] = v + m2[i1][j1];
      }
    delete l;
  }
  return m;
}
//=============================================================================
void
CExampleBox::Draw( 
  int r0,     // исходный ранг
  int r1,     // конечный ранг
  int bDither // делать "расфокусировку"?
  )
{
  int M0 = ( 1 << r1 );       // коэффициент квантования
  int MX = ( 1 << r0 ) - 1;   // максимальное значение

  int dr = r1 - r0;           // добавочный ранг
  int mr = 1 << ( dr >> 1 );  // размер матрицы возбуждения
  if( r0 == 1 )
    mr <<= 1;
  int msk = ( 1 << dr ) - 1;  // маска для остатка
  int ims = mr - 1;           // маска для индекса

  int* Mtx = MkMtx( mr );     // матрица возбуждения для остатка

  CRect r;
  GetClientRect( &r );
  CDC* pDC = GetDC();
  CDC  bmpDC;
  CBitmap bmp;
  bmp.CreateCompatibleBitmap( pDC, r.Width(), r.Height());
  bmpDC.CreateCompatibleDC( pDC );
  bmpDC.SelectObject( &bmp );

  for( int i = 0 ; i < r.bottom ; ++i )
    for( int j = 0 ; j < r.right ; ++j )
    {
      double D = double(j)/r.Width(); // исходная яркость в точке j
      int I = D * M0;            // квантованная яркость 
      int H = I >> dr;                // квантованная яркость в исходном разрешении 
      int L = I & msk;                // остаток
      if( dr && bDither )
      {
        int i1 = i & ims;       // индекс строки  в матрице возбуждения 
        int j1 = j & ims;       // индекс столбца в матрице возбуждения
        if( r0 == 1 )
          H = I >= Mtx[i1*mr+j1];  
        else
        if( L >= Mtx[i1*mr+j1] ) // коррекция яркости по остатку
          H += 1;
      }
      H = min( H, MX );         // ограничение по максимуму

      if( r0 < 8 )              // растяжка на диапазон 0 - 255 
        H = H * 255. / MX;      // исключительно для целей демострации 

      COLORREF c = RGB( H, H, H );
      bmpDC.SetPixel( j, i, c );
    }

  delete Mtx;
  pDC->BitBlt( 0, 0, r.Width(), r.Height(), &bmpDC, 0, 0, SRCCOPY );
}
//=============================================================================
