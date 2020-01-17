Imports System.Drawing

Public NotInheritable Class MeteorColorScheme
    Private mPrimaryColor As Color
    Private mSecondaryColor As Color

    Public Enum PrimaryColorEnum As Byte
        MetroNavyBlue
        MetroPurpleHeart
        MetroSelectiveYellow
        MetroDogerBlue
        MetroCitrus
        MetroTeal
        MetroDenim
        MetroRoyalBlue
        MetroMosque
        MetroMayaBlue
        MetroMalibu
        MetroMaroon
        MetroDarkTurquoise
        MetroDarkPastelGreen
    End Enum

    Public Sub New(ByVal primColor As PrimaryColorEnum)

    End Sub

#Region "Metro"
    Public Shared ReadOnly MetroNavyBlue As Color = Color.FromArgb(0, 106, 193)
    Public Shared ReadOnly MetroPurpleHeart As Color = Color.FromArgb(105, 27, 184)
    Public Shared ReadOnly MetroSelectiveYellow As Color = Color.FromArgb(244, 179, 0)
    Public Shared ReadOnly MetroDodgerBlue As Color = Color.FromArgb(31, 174, 255)
    Public Shared ReadOnly MetroCitrus As Color = Color.FromArgb(120, 186, 0)
    Public Shared ReadOnly MetroTeal As Color = Color.FromArgb(0, 130, 135)
    Public Shared ReadOnly MetroDenim As Color = Color.FromArgb(27, 88, 184)
    Public Shared ReadOnly MetroRoyalBlue As Color = Color.FromArgb(38, 115, 236)
    Public Shared ReadOnly MetroMosque As Color = Color.FromArgb(0, 77, 96)
    Public Shared ReadOnly MetroMayaBlue As Color = Color.FromArgb(86, 197, 255)
    Public Shared ReadOnly MetroMalibu As Color = Color.FromArgb(86, 156, 227)
    Public Shared ReadOnly MetroMaroon As Color = Color.FromArgb(99, 47, 0)
    Public Shared ReadOnly MetroDarkTurquoise As Color = Color.FromArgb(0, 216, 204)
    Public Shared ReadOnly MetroDarkPastelGreen As Color = Color.FromArgb(0, 193, 63)
    Friend Shared ReadOnly MetroPersianGreen As Color = Color.FromArgb(0, 170, 170)
    Friend Shared ReadOnly MetroFreeSpeechRed As Color = Color.FromArgb(176, 30, 0)
    Friend Shared ReadOnly MetroForestGreen As Color = Color.FromArgb(21, 153, 42)
    Friend Shared ReadOnly MetroDarkTangerine As Color = Color.FromArgb(255, 152, 29)
    Friend Shared ReadOnly MetroLima As Color = Color.FromArgb(131, 186, 31)
    Friend Shared ReadOnly MetroTyrianPurple As Color = Color.FromArgb(78, 0, 56)
    Friend Shared ReadOnly MetroChocolate As Color = Color.FromArgb(229, 108, 25)
    Friend Shared ReadOnly MetroGoldenPoppy As Color = Color.FromArgb(225, 183, 0)
    Friend Shared ReadOnly MetroEggplant As Color = Color.FromArgb(193, 0, 79)
    Friend Shared ReadOnly MetroScarlet As Color = Color.FromArgb(255, 46, 18)
    Friend Shared ReadOnly MetroWisteria As Color = Color.FromArgb(211, 157, 217)
    Friend Shared ReadOnly MetroDarkViolet As Color = Color.FromArgb(114, 0, 172)
    Friend Shared ReadOnly MetroTickleMePink As Color = Color.FromArgb(255, 118, 140)
    Friend Shared ReadOnly MetroDeepPink As Color = Color.FromArgb(255, 29, 119)
    Friend Shared ReadOnly MetroFreeSpeechMagenta As Color = Color.FromArgb(224, 100, 183)
    Friend Shared ReadOnly MetroMediumVioletRed As Color = Color.FromArgb(184, 27, 108)
    Friend Shared ReadOnly MetroLightSlateBlue As Color = Color.FromArgb(170, 64, 255)
    Friend Shared ReadOnly MetroPumpkin As Color = Color.FromArgb(255, 125, 35)
#End Region

#Region "Material"
    Friend Shared ReadOnly MaterialRed100 As Color = Color.FromArgb(255, 205, 210)
    Friend Shared ReadOnly MaterialRed200 As Color = Color.FromArgb(239, 154, 154)
    Friend Shared ReadOnly MaterialRed300 As Color = Color.FromArgb(229, 115, 115)
    Friend Shared ReadOnly MaterialRed400 As Color = Color.FromArgb(239, 83, 80)
    Friend Shared ReadOnly MaterialRed500 As Color = Color.FromArgb(244, 67, 54)
    Friend Shared ReadOnly MaterialRed600 As Color = Color.FromArgb(229, 57, 53)
    Friend Shared ReadOnly MaterialRed700 As Color = Color.FromArgb(211, 47, 47)
    Friend Shared ReadOnly MaterialRed800 As Color = Color.FromArgb(198, 40, 40)
    Friend Shared ReadOnly MaterialRed900 As Color = Color.FromArgb(183, 28, 28)

    Friend Shared ReadOnly MaterialPink100 As Color = Color.FromArgb(248, 187, 208)
    Friend Shared ReadOnly MaterialPink200 As Color = Color.FromArgb(244, 143, 177)
    Friend Shared ReadOnly MaterialPink300 As Color = Color.FromArgb(240, 98, 146)
    Friend Shared ReadOnly MaterialPink400 As Color = Color.FromArgb(236, 64, 122)
    Friend Shared ReadOnly MaterialPink500 As Color = Color.FromArgb(233, 30, 99)
    Friend Shared ReadOnly MaterialPink600 As Color = Color.FromArgb(216, 27, 96)
    Friend Shared ReadOnly MaterialPink700 As Color = Color.FromArgb(194, 24, 91)
    Friend Shared ReadOnly MaterialPink800 As Color = Color.FromArgb(173, 20, 87)
    Friend Shared ReadOnly MaterialPink900 As Color = Color.FromArgb(136, 14, 79)

    Friend Shared ReadOnly MaterialPruple100 As Color = Color.FromArgb(225, 190, 231)
    Friend Shared ReadOnly MaterialPruple200 As Color = Color.FromArgb(206, 147, 216)
    Friend Shared ReadOnly MaterialPruple300 As Color = Color.FromArgb(186, 104, 200)
    Friend Shared ReadOnly MaterialPruple400 As Color = Color.FromArgb(171, 71, 188)
    Friend Shared ReadOnly MaterialPruple500 As Color = Color.FromArgb(156, 39, 176)
    Friend Shared ReadOnly MaterialPruple600 As Color = Color.FromArgb(142, 36, 170)
    Friend Shared ReadOnly MaterialPruple700 As Color = Color.FromArgb(123, 31, 162)
    Friend Shared ReadOnly MaterialPruple800 As Color = Color.FromArgb(106, 27, 154)
    Friend Shared ReadOnly MaterialPruple900 As Color = Color.FromArgb(74, 20, 140)

    Friend Shared ReadOnly MaterialDeepPruple100 As Color = Color.FromArgb(209, 196, 233)
    Friend Shared ReadOnly MaterialDeepPruple200 As Color = Color.FromArgb(179, 157, 219)
    Friend Shared ReadOnly MaterialDeepPruple300 As Color = Color.FromArgb(149, 117, 205)
    Friend Shared ReadOnly MaterialDeepPruple400 As Color = Color.FromArgb(126, 87, 194)
    Friend Shared ReadOnly MaterialDeepPruple500 As Color = Color.FromArgb(103, 58, 183)
    Friend Shared ReadOnly MaterialDeepPruple600 As Color = Color.FromArgb(94, 53, 177)
    Friend Shared ReadOnly MaterialDeepPruple700 As Color = Color.FromArgb(81, 45, 168)
    Friend Shared ReadOnly MaterialDeepPruple800 As Color = Color.FromArgb(69, 39, 160)
    Friend Shared ReadOnly MaterialDeepPruple900 As Color = Color.FromArgb(49, 27, 146)

    Friend Shared ReadOnly MaterialIndigo100 As Color = Color.FromArgb(197, 202, 233)
    Friend Shared ReadOnly MaterialIndigo200 As Color = Color.FromArgb(159, 168, 218)
    Friend Shared ReadOnly MaterialIndigo300 As Color = Color.FromArgb(121, 134, 203)
    Friend Shared ReadOnly MaterialIndigo400 As Color = Color.FromArgb(92, 107, 192)
    Friend Shared ReadOnly MaterialIndigo500 As Color = Color.FromArgb(63, 81, 181)
    Friend Shared ReadOnly MaterialIndigo600 As Color = Color.FromArgb(57, 73, 171)
    Friend Shared ReadOnly MaterialIndigo700 As Color = Color.FromArgb(48, 63, 159)
    Friend Shared ReadOnly MaterialIndigo800 As Color = Color.FromArgb(40, 53, 147)
    Friend Shared ReadOnly MaterialIndigo900 As Color = Color.FromArgb(26, 35, 126)

    Friend Shared ReadOnly MaterialBlue100 As Color = Color.FromArgb(187, 222, 251)
    Friend Shared ReadOnly MaterialBlue200 As Color = Color.FromArgb(144, 202, 249)
    Friend Shared ReadOnly MaterialBlue300 As Color = Color.FromArgb(100, 181, 246)
    Friend Shared ReadOnly MaterialBlue400 As Color = Color.FromArgb(66, 165, 245)
    Friend Shared ReadOnly MaterialBlue500 As Color = Color.FromArgb(33, 150, 243)
    Friend Shared ReadOnly MaterialBlue600 As Color = Color.FromArgb(30, 136, 229)
    Friend Shared ReadOnly MaterialBlue700 As Color = Color.FromArgb(25, 118, 210)
    Friend Shared ReadOnly MaterialBlue800 As Color = Color.FromArgb(21, 101, 192)
    Friend Shared ReadOnly MaterialBlue900 As Color = Color.FromArgb(13, 71, 161)

    Friend Shared ReadOnly MaterialLightBlue100 As Color = Color.FromArgb(179, 229, 252)
    Friend Shared ReadOnly MaterialLightBlue200 As Color = Color.FromArgb(129, 212, 250)
    Friend Shared ReadOnly MaterialLightBlue300 As Color = Color.FromArgb(79, 195, 247)
    Friend Shared ReadOnly MaterialLightBlue400 As Color = Color.FromArgb(41, 182, 246)
    Friend Shared ReadOnly MaterialLightBlue500 As Color = Color.FromArgb(3, 169, 244)
    Friend Shared ReadOnly MaterialLightBlue600 As Color = Color.FromArgb(3, 155, 229)
    Friend Shared ReadOnly MaterialLightBlue700 As Color = Color.FromArgb(2, 136, 209)
    Friend Shared ReadOnly MaterialLightBlue800 As Color = Color.FromArgb(2, 119, 189)
    Friend Shared ReadOnly MaterialLightBlue900 As Color = Color.FromArgb(1, 87, 155)

    Friend Shared ReadOnly MaterialCyan100 As Color = Color.FromArgb(178, 235, 242)
    Friend Shared ReadOnly MaterialCyan200 As Color = Color.FromArgb(128, 222, 234)
    Friend Shared ReadOnly MaterialCyan300 As Color = Color.FromArgb(77, 208, 225)
    Friend Shared ReadOnly MaterialCyan400 As Color = Color.FromArgb(38, 198, 218)
    Friend Shared ReadOnly MaterialCyan500 As Color = Color.FromArgb(0, 188, 212)
    Friend Shared ReadOnly MaterialCyan600 As Color = Color.FromArgb(0, 172, 193)
    Friend Shared ReadOnly MaterialCyan700 As Color = Color.FromArgb(0, 151, 167)
    Friend Shared ReadOnly MaterialCyan800 As Color = Color.FromArgb(0, 131, 143)
    Friend Shared ReadOnly MaterialCyan900 As Color = Color.FromArgb(0, 96, 100)

    Friend Shared ReadOnly MaterialTeal100 As Color = Color.FromArgb(178, 223, 219)
    Friend Shared ReadOnly MaterialTeal200 As Color = Color.FromArgb(128, 203, 196)
    Friend Shared ReadOnly MaterialTeal300 As Color = Color.FromArgb(77, 182, 172)
    Friend Shared ReadOnly MaterialTeal400 As Color = Color.FromArgb(38, 166, 154)
    Friend Shared ReadOnly MaterialTeal500 As Color = Color.FromArgb(0, 150, 136)
    Friend Shared ReadOnly MaterialTeal600 As Color = Color.FromArgb(0, 137, 123)
    Friend Shared ReadOnly MaterialTeal700 As Color = Color.FromArgb(0, 121, 107)
    Friend Shared ReadOnly MaterialTeal800 As Color = Color.FromArgb(0, 105, 92)
    Friend Shared ReadOnly MaterialTeal900 As Color = Color.FromArgb(0, 77, 64)

    Friend Shared ReadOnly MaterialGreen100 As Color = Color.FromArgb(200, 230, 201)
    Friend Shared ReadOnly MaterialGreen200 As Color = Color.FromArgb(165, 214, 167)
    Friend Shared ReadOnly MaterialGreen300 As Color = Color.FromArgb(129, 199, 132)
    Friend Shared ReadOnly MaterialGreen400 As Color = Color.FromArgb(102, 187, 106)
    Friend Shared ReadOnly MaterialGreen500 As Color = Color.FromArgb(76, 175, 80)
    Friend Shared ReadOnly MaterialGreen600 As Color = Color.FromArgb(67, 160, 71)
    Friend Shared ReadOnly MaterialGreen700 As Color = Color.FromArgb(56, 142, 60)
    Friend Shared ReadOnly MaterialGreen800 As Color = Color.FromArgb(46, 125, 50)
    Friend Shared ReadOnly MaterialGreen900 As Color = Color.FromArgb(27, 94, 32)

    Friend Shared ReadOnly MaterialLightGreen100 As Color = Color.FromArgb(220, 237, 200)
    Friend Shared ReadOnly MaterialLightGreen200 As Color = Color.FromArgb(197, 225, 165)
    Friend Shared ReadOnly MaterialLightGreen300 As Color = Color.FromArgb(174, 213, 129)
    Friend Shared ReadOnly MaterialLightGreen400 As Color = Color.FromArgb(156, 204, 101)
    Friend Shared ReadOnly MaterialLightGreen500 As Color = Color.FromArgb(139, 195, 74)
    Friend Shared ReadOnly MaterialLightGreen600 As Color = Color.FromArgb(124, 179, 66)
    Friend Shared ReadOnly MaterialLightGreen700 As Color = Color.FromArgb(104, 159, 56)
    Friend Shared ReadOnly MaterialLightGreen800 As Color = Color.FromArgb(85, 139, 47)
    Friend Shared ReadOnly MaterialLightGreen900 As Color = Color.FromArgb(51, 105, 30)

    Friend Shared ReadOnly MaterialLime100 As Color = Color.FromArgb(240, 244, 195)
    Friend Shared ReadOnly MaterialLime200 As Color = Color.FromArgb(230, 238, 156)
    Friend Shared ReadOnly MaterialLime300 As Color = Color.FromArgb(220, 231, 117)
    Friend Shared ReadOnly MaterialLime400 As Color = Color.FromArgb(212, 225, 87)
    Friend Shared ReadOnly MaterialLime500 As Color = Color.FromArgb(205, 220, 57)
    Friend Shared ReadOnly MaterialLime600 As Color = Color.FromArgb(192, 202, 51)
    Friend Shared ReadOnly MaterialLime700 As Color = Color.FromArgb(175, 180, 43)
    Friend Shared ReadOnly MaterialLime800 As Color = Color.FromArgb(158, 157, 36)
    Friend Shared ReadOnly MaterialLime900 As Color = Color.FromArgb(130, 119, 23)

    Friend Shared ReadOnly MaterialYellow100 As Color = Color.FromArgb(255, 249, 196)
    Friend Shared ReadOnly MaterialYellow200 As Color = Color.FromArgb(255, 245, 157)
    Friend Shared ReadOnly MaterialYellow300 As Color = Color.FromArgb(255, 241, 118)
    Friend Shared ReadOnly MaterialYellow400 As Color = Color.FromArgb(255, 238, 88)
    Friend Shared ReadOnly MaterialYellow500 As Color = Color.FromArgb(255, 235, 59)
    Friend Shared ReadOnly MaterialYellow600 As Color = Color.FromArgb(253, 216, 53)
    Friend Shared ReadOnly MaterialYellow700 As Color = Color.FromArgb(251, 192, 45)
    Friend Shared ReadOnly MaterialYellow800 As Color = Color.FromArgb(249, 168, 37)
    Friend Shared ReadOnly MaterialYellow900 As Color = Color.FromArgb(245, 127, 23)

    Friend Shared ReadOnly MaterialAmber100 As Color = Color.FromArgb(255, 236, 179)
    Friend Shared ReadOnly MaterialAmber200 As Color = Color.FromArgb(255, 224, 130)
    Friend Shared ReadOnly MaterialAmber300 As Color = Color.FromArgb(255, 213, 79)
    Friend Shared ReadOnly MaterialAmber400 As Color = Color.FromArgb(255, 202, 40)
    Friend Shared ReadOnly MaterialAmber500 As Color = Color.FromArgb(255, 193, 7)
    Friend Shared ReadOnly MaterialAmber600 As Color = Color.FromArgb(255, 179, 0)
    Friend Shared ReadOnly MaterialAmber700 As Color = Color.FromArgb(255, 160, 0)
    Friend Shared ReadOnly MaterialAmber800 As Color = Color.FromArgb(255, 143, 0)
    Friend Shared ReadOnly MaterialAmber900 As Color = Color.FromArgb(255, 111, 0)

    Friend Shared ReadOnly MaterialOrange100 As Color = Color.FromArgb(255, 224, 178)
    Friend Shared ReadOnly MaterialOrange200 As Color = Color.FromArgb(255, 204, 128)
    Friend Shared ReadOnly MaterialOrange300 As Color = Color.FromArgb(255, 183, 77)
    Friend Shared ReadOnly MaterialOrange400 As Color = Color.FromArgb(255, 167, 38)
    Friend Shared ReadOnly MaterialOrange500 As Color = Color.FromArgb(255, 152, 0)
    Friend Shared ReadOnly MaterialOrange600 As Color = Color.FromArgb(251, 140, 0)
    Friend Shared ReadOnly MaterialOrange700 As Color = Color.FromArgb(245, 124, 0)
    Friend Shared ReadOnly MaterialOrange800 As Color = Color.FromArgb(239, 108, 0)
    Friend Shared ReadOnly MaterialOrange900 As Color = Color.FromArgb(230, 81, 0)

    Friend Shared ReadOnly MaterialDeepOrange100 As Color = Color.FromArgb(255, 204, 188)
    Friend Shared ReadOnly MaterialDeepOrange200 As Color = Color.FromArgb(255, 171, 145)
    Friend Shared ReadOnly MaterialDeepOrange300 As Color = Color.FromArgb(255, 138, 101)
    Friend Shared ReadOnly MaterialDeepOrange400 As Color = Color.FromArgb(255, 112, 67)
    Friend Shared ReadOnly MaterialDeepOrange500 As Color = Color.FromArgb(255, 87, 34)
    Friend Shared ReadOnly MaterialDeepOrange600 As Color = Color.FromArgb(244, 81, 30)
    Friend Shared ReadOnly MaterialDeepOrange700 As Color = Color.FromArgb(230, 74, 25)
    Friend Shared ReadOnly MaterialDeepOrange800 As Color = Color.FromArgb(216, 67, 21)
    Friend Shared ReadOnly MaterialDeepOrange900 As Color = Color.FromArgb(191, 54, 12)

    Friend Shared ReadOnly MaterialBrown100 As Color = Color.FromArgb(215, 204, 200)
    Friend Shared ReadOnly MaterialBrown200 As Color = Color.FromArgb(188, 170, 164)
    Friend Shared ReadOnly MaterialBrown300 As Color = Color.FromArgb(161, 136, 127)
    Friend Shared ReadOnly MaterialBrown400 As Color = Color.FromArgb(141, 110, 99)
    Friend Shared ReadOnly MaterialBrown500 As Color = Color.FromArgb(121, 85, 72)
    Friend Shared ReadOnly MaterialBrown600 As Color = Color.FromArgb(109, 76, 65)
    Friend Shared ReadOnly MaterialBrown700 As Color = Color.FromArgb(93, 64, 55)
    Friend Shared ReadOnly MaterialBrown800 As Color = Color.FromArgb(78, 52, 46)
    Friend Shared ReadOnly MaterialBrown900 As Color = Color.FromArgb(62, 39, 35)

    Friend Shared ReadOnly MaterialGrey100 As Color = Color.FromArgb(245, 245, 245)
    Friend Shared ReadOnly MaterialGrey200 As Color = Color.FromArgb(238, 238, 238)
    Friend Shared ReadOnly MaterialGrey300 As Color = Color.FromArgb(224, 224, 224)
    Friend Shared ReadOnly MaterialGrey400 As Color = Color.FromArgb(189, 189, 189)
    Friend Shared ReadOnly MaterialGrey500 As Color = Color.FromArgb(158, 158, 158)
    Friend Shared ReadOnly MaterialGrey600 As Color = Color.FromArgb(117, 117, 117)
    Friend Shared ReadOnly MaterialGrey700 As Color = Color.FromArgb(97, 97, 97)
    Friend Shared ReadOnly MaterialGrey800 As Color = Color.FromArgb(66, 66, 66)
    Friend Shared ReadOnly MaterialGrey900 As Color = Color.FromArgb(33, 33, 33)

    Friend Shared ReadOnly MaterialBlueGrey100 As Color = Color.FromArgb(207, 216, 220)
    Friend Shared ReadOnly MaterialBlueGrey200 As Color = Color.FromArgb(176, 190, 197)
    Friend Shared ReadOnly MaterialBlueGrey300 As Color = Color.FromArgb(144, 164, 174)
    Friend Shared ReadOnly MaterialBlueGrey400 As Color = Color.FromArgb(120, 144, 156)
    Friend Shared ReadOnly MaterialBlueGrey500 As Color = Color.FromArgb(96, 125, 139)
    Friend Shared ReadOnly MaterialBlueGrey600 As Color = Color.FromArgb(84, 110, 122)
    Friend Shared ReadOnly MaterialBlueGrey700 As Color = Color.FromArgb(69, 90, 100)
    Friend Shared ReadOnly MaterialBlueGrey800 As Color = Color.FromArgb(55, 71, 79)
    Friend Shared ReadOnly MaterialBlueGrey900 As Color = Color.FromArgb(38, 50, 56)
#End Region




End Class
