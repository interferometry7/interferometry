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
    AfxMessageBox( "������������ �������� Range 0", MB_ICONERROR );
    return;
  }

  if( R1 < R0 || R1 > 16 )
  {
    AfxMessageBox( "������������ �������� Range 1", MB_ICONERROR );
    return;
  }

  int t = R1 - R0;
  if( R0 > 1 && ( t & 1 ))
  {
    AfxMessageBox( "������������ ��������� Range 0 � Range 1", MB_ICONERROR );
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
  int r0,     // �������� ����
  int r1,     // �������� ����
  int bDither // ������ "��������������"?
  )
{
  int M0 = ( 1 << r1 );       // ����������� �����������
  int MX = ( 1 << r0 ) - 1;   // ������������ ��������

  int dr = r1 - r0;           // ���������� ����
  int mr = 1 << ( dr >> 1 );  // ������ ������� �����������
  if( r0 == 1 )
    mr <<= 1;
  int msk = ( 1 << dr ) - 1;  // ����� ��� �������
  int ims = mr - 1;           // ����� ��� �������

  int* Mtx = MkMtx( mr );     // ������� ����������� ��� �������

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
      double D = double(j)/r.Width(); // �������� ������� � ����� j
      int I = D * M0;            // ������������ ������� 
      int H = I >> dr;                // ������������ ������� � �������� ���������� 
      int L = I & msk;                // �������
      if( dr && bDither )
      {
        int i1 = i & ims;       // ������ ������  � ������� ����������� 
        int j1 = j & ims;       // ������ ������� � ������� �����������
        if( r0 == 1 )
          H = I >= Mtx[i1*mr+j1];  
        else
        if( L >= Mtx[i1*mr+j1] ) // ��������� ������� �� �������
          H += 1;
      }
      H = min( H, MX );         // ����������� �� ���������

      if( r0 < 8 )              // �������� �� �������� 0 - 255 
        H = H * 255. / MX;      // ������������� ��� ����� ����������� 

      COLORREF c = RGB( H, H, H );
      bmpDC.SetPixel( j, i, c );
    }

  delete Mtx;
  pDC->BitBlt( 0, 0, r.Width(), r.Height(), &bmpDC, 0, 0, SRCCOPY );
}
//=============================================================================
