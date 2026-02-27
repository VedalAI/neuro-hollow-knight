namespace HollowNeuro

type Reachability =
    // always show
    | Always
    // show if visited
    | Visited
    // dont show but allow using for pathfinding
    | Passthru
    // never show
    | Never

type Dir =
    // circle (atan2) start at E and goes towards N
    | E = 0
    | Nee = 1
    | Ne = 2
    | Nne = 3
    | N = 4
    | Nnw = 5
    | Nw = 6
    | Nww = 7
    | W = 8
    | Sww = 9
    | Sw = 10
    | Ssw = 11
    | S = 12
    | Sse = 13
    | Se = 14
    | See = 15

module Generated =
    let scenePos s =
        match s with
        | "Crossroads_01" -> -1.11299974, 6.6139998
        | "Crossroads_02" -> 0.34699800000000003, 6.6100007000000005
        | "Crossroads_03" -> 2.0239991600000002, 4.9490013
        | "Crossroads_04" -> 3.72899806, 3.4459999
        | "Crossroads_04_b" -> 3.7290007600000004, 3.4459999
        | "Crossroads_05" -> -1.26500097, 5.8750009
        | "Crossroads_06" -> -1.5030011399999998, 5.105
        | "Crossroads_07" -> -2.21700194, 5.8440008
        | "Crossroads_08" -> -1.45299974, 4.23300075
        | "Crossroads_09" -> -3.12800144, 4.81500005
        | "Crossroads_10" -> -0.58600064, 5.0990009
        | "Crossroads_11_alt" -> -3.7180011399999997, 5.7650003000000005
        | "Crossroads_12" -> -3.15000064, 4.3130002
        | "Crossroads_13" -> -0.36400101, 4.05800056
        | "Crossroads_14" -> 2.62800006, 6.1869998
        | "Crossroads_15" -> 2.80399966, 4.9030013
        | "Crossroads_16" -> 1.62399806, 5.9189997
        | "Crossroads_18_b" -> -2.20000154, 3.713001
        | "Crossroads_18" -> -2.20000154, 3.713001
        | "Crossroads_19" -> 2.07199876, 3.86200046
        | "Crossroads_21" -> 0.86599926, 4.88500118
        | "Crossroads_22" -> 0.7459995600000001, 5.3660001699999995
        | "Crossroads_25" -> -3.13100084, 5.3090005
        | "Crossroads_27" -> 3.54699986, 4.348001
        | "Crossroads_30" -> -0.64399976, 4.44300079
        | "Crossroads_31" -> 2.83099986, 4.441000456
        | "Crossroads_33" -> -2.22700174, 4.6390004099999995
        | "Crossroads_35" -> -4.335000340000001, 3.93799996
        | "Crossroads_36" -> -3.99300124, 4.9640007
        | "Crossroads_37" -> -0.75200046, 3.7030008
        | "Crossroads_38" -> -3.09800224, 6.4879999
        | "Crossroads_39" -> 1.6659991600000001, 6.5240007
        | "Crossroads_40" -> 0.15099886, 5.8990006
        | "Crossroads_42" -> 0.94099886, 4.0880003
        | "Crossroads_43" -> 0.95299816, 3.70800065
        | "Crossroads_45" -> 3.45899886, 5.9520006
        | "Crossroads_46" -> 4.741000359999999, 4.18199968
        | "Crossroads_47" -> 1.4649987599999998, 4.464000696
        | "Crossroads_48" -> 3.3289992600000002, 6.5270004
        | "Crossroads_49" -> 0.03299844000000002, 2.9580002
        | "Crossroads_52" -> -1.6670009399999999, 3.5380006
        | "Mines_33" -> 3.0899990600000002, 5.29399967
        | "Crossroads_35_b" -> -4.335000340000001, 3.93800115
        | "Crossroads_21_b" -> 0.86599926, 4.88499832
        | "Crossroads_ShamanTemple" -> -1.5030011399999998, 5.105
        | "Abyss_01" -> 8.378000199999999, -5.19500021
        | "Waterways_01" -> 5.11099954, -4.171
        | "Waterways_02" -> 5.526001399999999, -5.0110006700000005
        | "Waterways_02_b" -> 3.9160009999999996, -5.0540004000000005
        | "Waterways_03" -> 7.0820035, -3.9210008000000003
        | "Waterways_04" -> 2.6890009999999998, -4.478000700000001
        | "Waterways_04_part_b" -> 3.869001, -4.7160004
        | "Waterways_04b" -> 0.39400049999999975, -4.4420004
        | "Waterways_05" -> 7.2140026, -4.46400074
        | "Waterways_06" -> 6.959002, -5.76200108
        | "Waterways_07" -> 9.4940023, -5.433000600000001
        | "Waterways_08" -> 4.5310025, -5.81599907
        | "Waterways_09" -> -1.0760000000000005, -4.49700026
        | "Waterways_12" -> 2.8960009999999996, -5.9440004
        | "Waterways_13" -> 11.2889986, -5.48299936
        | "Waterways_14" -> 10.871002, -4.4460006000000005
        | "Waterways_15" -> 7.710001, -4.8110004
        | "Abyss_02" -> 6.548998299999999, -6.6560007
        | "GG_Pipeway" -> 2.3140009999999998, -5.2450004
        | "GG_Waterways" -> -0.054999000000000464, -5.6950004000000005
        | "Cliffs_01" -> -11.3429985, 9.10399886
        | "Cliffs_01_b" -> -10.382, 8.234999
        | "Cliffs_02" -> -8.095000299999999, 9.33399936
        | "Cliffs_02_b" -> -8.094997399999999, 9.33399936
        | "Cliffs_04" -> -9.265998878, 8.75399943
        | "Cliffs_05" -> -7.828999999999999, 8.75399943
        | "Cliffs_06" -> -9.398, 8.158999000000001
        | "Cliffs_06_b" -> -8.837, 8.125999
        | "Fungus1_28" -> -9.053998989999998, 7.2639978
        | "Fungus1_28_b" -> -9.053998989999998, 7.2639997
        | "Abyss_03_c" -> 10.875001, -7.2099994
        | "Deepnest_East_01" -> 12.879000699999999, -6.1719992999999995
        | "Deepnest_East_02" -> 13.6120015, -5.0329994
        | "Deepnest_East_03" -> 15.33399774, -3.99799955
        | "Deepnest_East_04" -> 16.3664017, -1.6636997999999998
        | "Deepnest_East_06" -> 18.591001499999997, -4.90199984
        | "Deepnest_East_07" -> 15.290999419999999, -1.1989997999999997
        | "Deepnest_East_08" -> 13.6779985, 0.28699929999999974
        | "Deepnest_East_09" -> 13.252999299999999, 0.9220005000000002
        | "Deepnest_East_10" -> 19.064003, -4.26899946
        | "Deepnest_East_11" -> 17.6989995, -3.5830000699999998
        | "Deepnest_East_12" -> 19.7759991, -2.4830006
        | "Deepnest_East_13" -> 18.1779995, -2.2070005999999998
        | "Deepnest_East_16" -> 21.673001499999998, -4.906
        | "Deepnest_East_14b" -> 19.7580015, -5.6909989
        | "Deepnest_East_14" -> 20.5590015, -5.6909989
        | "Deepnest_East_15" -> 17.1310015, -1.2429999999999999
        | "Deepnest_East_Hornet" -> 21.3510018, -3.2129994699999997
        | "Deepnest_East_Hornet_b" -> 21.3480015, -3.2129994699999997
        | "Hive_01" -> 14.4190025, -7.355999799999999
        | "Hive_01_b" -> 14.4190025, -7.355999799999999
        | "Hive_02" -> 16.443001499999998, -7.157
        | "Hive_03" -> 14.3590015, -6.042
        | "Hive_03_c" -> 14.3589983, -6.381
        | "Hive_05" -> 18.2031021, -6.1981996
        | "Hive_04" -> 16.5630015, -5.843
        | "Deepnest_East_02_b" -> 13.611996699999999, -5.03300083
        | "Deepnest_East_09_b" -> 13.252999299999999, 0.922002
        | "Hive_04_b" -> 16.5760002, -5.8430003
        | "Hive_03_b" -> 13.7470015, -5.6
        | "Deepnest_East_18" -> 17.7650015, -4.373
        | "GG_Lurker" -> 18.1400015, 0.2999999999999998
        | "Fungus1_01" -> -6.0999976, 5.8290003
        | "Fungus1_01b" -> -7.642998, 5.9549997
        | "Fungus1_02" -> -8.292998, 5.20899992
        | "Fungus1_03" -> -10.4129969, 5.67800026
        | "Fungus1_04" -> -13.6529976, 6.135
        | "Fungus1_05" -> -10.4449974, 4.748998928
        | "Fungus1_06" -> -6.8079973, 4.703999805
        | "Fungus1_08" -> -5.567997, 4.2309997599999996
        | "Fungus1_09" -> -14.685999500000001, 3.85200005
        | "Fungus1_10" -> -9.6829973, 3.8189995999999997
        | "Fungus1_11" -> -7.624997700000001, 3.013
        | "Fungus1_12" -> -9.464997, 2.9579999999999997
        | "Fungus1_13" -> -11.49999583, 2.7329999999999997
        | "Fungus1_14" -> -9.489997500000001, 4.16000013
        | "Fungus1_14_b" -> -9.489997500000001, 4.16000013
        | "Fungus1_15" -> -17.205999, 3.98599916
        | "Fungus1_16_alt" -> -12.9759966, 5.8119990999999995
        | "Fungus1_17" -> -9.2169968, 5.69400005
        | "Fungus1_19" -> -7.674996500000001, 3.783
        | "Fungus1_20_v02" -> -11.53399622, 7.0199993
        | "Fungus1_21" -> -12.25799715, 6.4099997
        | "Fungus1_22" -> -12.39099848, 5.02699976
        | "Fungus1_25" -> -14.927998200000001, 5.846
        | "Fungus1_26" -> -16.4079987, 6.0410002
        | "Fungus1_29" -> -8.639999, 3.0329996
        | "Fungus1_30" -> -11.851997970000001, 3.8319997999999997
        | "Fungus1_31" -> -11.282997700000001, 5.09199934
        | "Fungus1_32" -> -10.8979994, 6.4739998
        | "Fungus1_34" -> -6.179998, 3.2579997
        | "Fungus1_37" -> -6.563998000000001, 3.5949999999999998
        | "Fungus1_Slug" -> -17.948995, 5.0659998
        | "Fungus1_07" -> -6.517996, 4.0349996599999995
        | "Fungus1_09_b" -> -14.685999500000001, 3.85199766
        | "Fungus3_01" -> -7.63300083, 1.7509998549999999
        | "Fungus3_02" -> -7.08599946, 0.4310003
        | "Fungus3_03" -> -8.2770004, 0.6809997399999999
        | "Fungus3_24" -> -8.867000560000001, 1.6970009099999999
        | "Fungus3_25" -> -6.4570007, 2.1160007
        | "Fungus3_25b" -> -4.8970003, 2.1520009499999997
        | "Fungus3_26" -> -3.9220003, 2.441001
        | "Fungus3_27" -> -4.8639993, 1.596000838
        | "Fungus3_28" -> -4.7570003, 2.6720004
        | "Fungus3_30" -> -9.3419981, 2.14900045
        | "Fungus3_35" -> -7.866999602, -0.3589998000000001
        | "Fungus3_47" -> -6.1849999, 1.3950004999999999
        | "Fungus3_44" -> -10.6670007, 1.3969999999999998
        | "Fungus2_01" -> -6.3329998, -0.64899969
        | "Fungus2_02" -> -7.203000599999999, -0.99499923
        | "Fungus2_34" -> -7.073001, -0.67900026
        | "Fungus2_03" -> -4.830998, -0.02999932000000005
        | "Fungus2_04" -> -3.60200005, 0.025000739999999966
        | "Fungus2_05" -> -3.064998676, 0.90500004
        | "Fungus2_06" -> -2.21299796, 2.0000009399999996
        | "Fungus2_07" -> -1.3129977, 0.76600044
        | "Fungus2_08" -> -0.4469984, 0.3790006400000001
        | "Fungus2_09" -> -1.3059996999999999, -0.08099920000000005
        | "Fungus2_10" -> -2.4559994, -0.5059998
        | "Fungus2_11" -> -2.81399946, -1.69699956
        | "Fungus2_12" -> -1.2289980999999999, -2.04400016
        | "Fungus2_13" -> -1.0259982, -3.2110002599999996
        | "Fungus2_14" -> -2.81599884, -2.9299991600000004
        | "Fungus2_14_b" -> -2.81599884, -2.9299991600000004
        | "Fungus2_14_c" -> -2.806999, -2.9299991600000004
        | "Fungus2_15" -> -3.38999753, -4.20799886
        | "Fungus2_17" -> -3.40199952, -2.26699956
        | "Fungus2_18" -> -4.3409992, -1.14400006
        | "Fungus2_19" -> -5.8309985, -1.87399886
        | "Fungus2_20" -> -6.856999399999999, -1.7730001199999998
        | "Fungus2_21" -> -0.6839988999999997, -0.55499966
        | "Fungus2_23" -> -1.8459976999999999, -4.47099966
        | "Fungus2_26" -> -1.2939979, 1.68600004
        | "Fungus2_28" -> -2.576999, 0.39500004
        | "Fungus2_29" -> -4.4069987, -2.1569994599999998
        | "Fungus2_29_b" -> -4.4070011000000004, -2.1569994599999998
        | "Fungus2_30" -> -4.8069997, -3.6919992600000002
        | "Fungus2_31" -> -2.33599954, -3.60099896
        | "Fungus2_32" -> 0.3580000000000001, -0.020999259999999964
        | "Fungus2_33" -> -3.0979996218, 2.1940006399999996
        | "Deepnest_01" -> -7.8609987, -1.70400012
        | "Fungus1_23" -> -14.3399987, 0.90900026
        | "Fungus1_24" -> -10.326000700000002, 0.16699956
        | "Fungus3_04" -> -12.2720017, 0.49600005999999996
        | "Fungus3_05" -> -11.494001, -0.30799993999999997
        | "Fungus3_08" -> -12.4170022, -1.19099974
        | "Fungus3_10" -> -12.957000240000001, -0.63200059
        | "Fungus3_11" -> -10.5540013, -0.8010008399999999
        | "Fungus3_13" -> -12.837997900000001, 0.363999699
        | "Fungus3_21" -> -13.23599957, 1.24299929
        | "Fungus3_22" -> -13.5840001, 2.45599946
        | "Fungus3_23" -> -14.7870021, 1.92599976
        | "Fungus3_23_b" -> -14.7870021, 1.92599976
        | "Fungus3_34" -> -10.5879998, 0.69099946
        | "Fungus3_39" -> -9.273, -1.43100074
        | "Fungus3_40" -> -14.7950005, -0.08700010000000002
        | "Deepnest_43" -> -14.3060012, -2.1570006399999997
        | "Fungus3_48_bot" -> -16.4599976, 1.3399977600000001
        | "Fungus3_48_top" -> -16.4599976, 1.3399977600000001
        | "Fungus3_48_left" -> -17.039001, 1.54799976
        | "Fungus3_48" -> -16.4600015, 1.33999916
        | "Fungus3_49" -> -13.53400086, 0.4400003
        | "Fungus3_22_b" -> -13.5840001, 2.45599946
        | "Fungus3_50" -> -15.046001, -1.14480024
        | "Deepnest_43_b" -> -14.3060012, -2.1569980400000004
        | "Abyss_03_b" -> -1.779001000000001, -7.2159977
        | "Deepnest_01b" -> -7.8600013, -2.7939998999999998
        | "Deepnest_02" -> -7.1129996, -2.8800000999999997
        | "Deepnest_03" -> -8.8840005, -6.2520003
        | "Deepnest_09" -> -17.534, -4.526
        | "Deepnest_10" -> -18.732999500000002, -5.9709992
        | "Deepnest_14" -> -8.9329993, -3.9550001
        | "Deepnest_16" -> -6.813001400000001, -4.105999
        | "Deepnest_17" -> -7.993999700000001, -4.0750003
        | "Deepnest_26" -> -11.584997860000001, -3.6859995999999997
        | "Deepnest_26b" -> -12.708, -3.6859995999999997
        | "Deepnest_30" -> -7.358000500000001, -5.9699993
        | "Deepnest_30_b" -> -7.358000500000001, -5.97000073
        | "Deepnest_31" -> -10.4260003, -7.4259997
        | "Deepnest_32" -> -7.863, -7.712999699999999
        | "Deepnest_33" -> -9.024000000000001, -4.8729997
        | "Deepnest_34" -> -10.677000000000001, -6.436999699999999
        | "Deepnest_35" -> -10.16, -5.0239997
        | "Deepnest_36" -> -6.3020000000000005, -2.8139996999999997
        | "Deepnest_37" -> -5.634, -7.045999699999999
        | "Deepnest_38" -> -4.5840000000000005, -6.4479997
        | "Deepnest_39" -> -13.817, -6.1359997
        | "Deepnest_42" -> -14.376000000000001, -4.4199997
        | "Deepnest_40" -> -11.843, -4.7679997
        | "Deepnest_41" -> -16.799, -6.4259997
        | "Deepnest_41_b" -> -16.7989995, -6.4259987
        | "Deepnest_44" -> -5.0040000000000004, -7.8359997
        | "Deepnest_44_b" -> -4.724, -7.835998
        | "Fungus2_25" -> -5.202999300000001, -4.91000031
        | "Room_Mask_Maker" -> -15.336, -3.4950003
        | "Tutorial_01" -> -5.4429989, 7.4689999
        | "Town" -> -1.92299925, 7.64899973
        | "Crossroads_46b" -> 6.583998, 4.18299964
        | "Crossroads_50" -> 7.158996500000001, 3.5879989
        | "RestingGrounds_02" -> 8.8589996, 4.186999827
        | "RestingGrounds_04" -> 11.09500064, 4.187999278
        | "RestingGrounds_05" -> 12.434996, 4.75799945
        | "RestingGrounds_06" -> 10.20899715, 3.6579998000000002
        | "RestingGrounds_08" -> 13.7749962, 5.8279987
        | "RestingGrounds_09" -> 13.170996, 4.9179993
        | "RestingGrounds_10" -> 12.7949985, 3.6689994
        | "RestingGrounds_10_b" -> 12.7949985, 3.6689994
        | "RestingGrounds_10_c" -> 14.643999, 3.6329994
        | "RestingGrounds_10_d" -> 13.798999, 3.7399994000000003
        | "RestingGrounds_12" -> 14.2499975, 4.1799994
        | "RestingGrounds_17" -> 11.521999000000001, 4.9709994
        | "Ruins2_10" -> 11.320999500000001, 2.9229999
        | "Mines_01" -> 4.2490001, 5.8729992
        | "Mines_02" -> 5.1920038, 5.0999994
        | "Mines_03" -> 5.8800011, 6.0449991999999995
        | "Mines_04" -> 7.4650002, 5.6239992
        | "Mines_05" -> 5.8600016, 7.28099923
        | "Mines_06" -> 4.3200014, 7.01499896
        | "Mines_07" -> 8.6930008, 5.8369994
        | "Mines_10" -> 1.5680018000000002, 7.69499831
        | "Mines_11" -> 5.8560028299999995, 8.376998
        | "Mines_13" -> 6.15700105, 9.2659979
        | "Mines_16" -> 0.6870016999999997, 7.24799876
        | "Mines_17" -> 6.6810036, 6.0559993
        | "Mines_18" -> 6.581000355, 8.31299977
        | "Mines_19" -> 6.5930014, 7.4359994
        | "Mines_20" -> 7.5570002, 7.8319994
        | "Mines_20_b" -> 7.5570002, 8.2949994
        | "Mines_23" -> 9.607002399999999, 9.4189997
        | "Mines_24" -> 11.570003400000001, 9.1879998
        | "Mines_25" -> 11.7020024, 10.3739996
        | "Mines_28" -> 10.5179997, 5.3679991000000005
        | "Mines_28_b" -> 10.5179997, 5.368001
        | "Mines_29" -> 6.8059993, 5.318999399999999
        | "Mines_30" -> 4.2530012, 7.7499991
        | "Mines_31" -> 10.2280035, 7.4299994
        | "Mines_32" -> 6.581000355, 8.6779986
        | "Mines_34" -> 10.4030009, 12.158999399999999
        | "Mines_36" -> 2.8400014, 7.0339994
        | "Mines_37" -> 7.5620014, 6.8099994
        | "Abyss_17" -> 2.9250017, -7.559997
        | "Abyss_03" -> 6.15300083, -7.2309965
        | "Abyss_04" -> 5.4020017000000005, -8.241997
        | "Abyss_05" -> 7.8010017000000005, -8.350997
        | "Abyss_06_Core" -> 4.8950017, -11.476997
        | "Abyss_06_Core_b" -> 4.8950033, -11.476997
        | "Abyss_08" -> 3.1100017, -11.987997
        | "Abyss_09" -> 9.8050017, -13.138997
        | "Abyss_10" -> 13.2950017, -13.577997
        | "Abyss_12" -> 2.6680017, -13.807997
        | "Abyss_16" -> 6.6740017, -13.717997
        | "Abyss_18" -> 2.8590017, -8.902997
        | "Abyss_18_b" -> 2.8590024, -8.9029955
        | "Abyss_19" -> -0.18699829999999995, -8.639997000000001
        | "Abyss_20" -> -0.14299830000000036, -9.789997
        | "Abyss_21" -> -2.6279983000000007, -9.048997
        | "Abyss_22" -> 10.0620017, -8.616997
        | "Crossroads_49b" -> 0.038998499999999936, 1.5589996
        | "Ruins1_01" -> 1.6849980000000002, -0.81500083
        | "Ruins1_02" -> 2.7139984999999998, -1.7300005999999999
        | "Ruins1_03" -> 3.2129972, -3.106
        | "Ruins1_04" -> 0.6479977999999997, -3.2660004
        | "Ruins1_05b" -> 5.0479951, -3.4210004
        | "Ruins1_05" -> 5.047995500000001, -2.3110002
        | "Ruins1_05c" -> 5.0479965, -2.3110014
        | "Ruins1_06" -> 3.6929967, -0.47900098
        | "Ruins1_09" -> 5.9979972, -1.00900054
        | "Ruins1_17" -> 2.1189975, -0.02400100000000005
        | "Ruins1_18" -> 6.44799467, -1.88600063
        | "Ruins1_18_b" -> 6.44799467, -1.88600063
        | "Ruins1_23" -> 6.0019955000000005, -0.09199995000000005
        | "Ruins1_24" -> 4.5239975, 1.0529997
        | "Ruins1_25" -> 6.71699615, 0.4640000000000001
        | "Ruins1_27" -> 6.43699785, -3.1209995999999998
        | "Ruins1_29" -> 2.5319975, 0.9129991000000001
        | "Ruins1_28" -> 1.2469975, 0.8379994999999999
        | "Ruins1_30" -> 5.8019948, 1.1440000000000001
        | "Ruins1_31_top" -> 5.0339989, -0.27099870000000004
        | "Ruins1_31_top_2" -> 5.0339995, -0.27099870000000004
        | "Ruins1_31" -> 5.0339995, -0.271
        | "Ruins1_31b" -> 4.788997500000001, 0.06499959999999994
        | "Ruins1_32" -> 3.4449975000000004, 0.7729997000000001
        | "Ruins2_01" -> 7.9559941, -2.7820006
        | "Ruins2_01_b" -> 7.95599504, -3.2780004
        | "Ruins2_03" -> 7.9659967, -0.7620002
        | "Ruins2_03b" -> 7.96599766, -0.7620002
        | "Ruins2_04" -> 9.9409942, -3.1790002
        | "Ruins2_05" -> 11.986997500000001, -2.1640004
        | "Ruins2_06" -> 11.8019981, -3.359
        | "Ruins2_07" -> 13.522995, -3.7490001
        | "Ruins2_07_left" -> 13.522999500000001, -3.7490001
        | "Ruins2_07_right" -> 13.522999500000001, -3.7490001
        | "Ruins2_09" -> 12.175997500000001, -1.2400004
        | "Ruins2_08" -> 12.784995, -2.9549997
        | "Ruins2_10b" -> 11.3129959, 0.15600010000000009
        | "Ruins2_11_b" -> 13.865996500000001, -2.2850004
        | "Ruins2_11" -> 13.862997, -2.2810004
        | "Ruins2_Watcher_Room" -> 8.07599636, 1.4100000000000001
        | "Ruins_Elevator" -> 9.9469975, -1.6039998500000001
        | "Ruins_Bathhouse" -> 10.4573993, -0.2890001
        | _ -> 0.0, 0.0

    let fullSceneNames =
        [| "Pre_Menu_Intro"
           "Menu_Title"
           "Quit_To_Menu"
           "BetaEnd"
           "Knight_Pickup"
           "Opening_Sequence"
           "Tutorial_01"
           "Town"
           "Cinematic_Stag_travel"
           "Room_Town_Stag_Station"
           "Room_Charm_Shop"
           "Room_Mender_House"
           "Room_mapper"
           "Room_nailmaster"
           "Room_nailmaster_02"
           "Room_nailmaster_03"
           "Room_nailsmith"
           "Room_shop"
           "Room_Sly_Storeroom"
           "Room_temple"
           "Room_ruinhouse"
           "Room_Mask_Maker"
           "Room_Mansion"
           "Room_Tram"
           "Room_Tram_RG"
           "Room_Bretta"
           "Room_Bretta_Basement"
           "Room_Fungus_Shaman"
           "Room_Ouiji"
           "Room_Jinn"
           "Room_Colosseum_01"
           "Room_Colosseum_02"
           "Room_Colosseum_Bronze"
           "Room_Colosseum_Silver"
           "Room_Colosseum_Gold"
           "Room_Colosseum_Spectate"
           "Room_Slug_Shrine"
           "Crossroads_01"
           "Crossroads_02"
           "Crossroads_03"
           "Crossroads_04"
           "Crossroads_05"
           "Crossroads_06"
           "Crossroads_07"
           "Crossroads_08"
           "Crossroads_09"
           "Crossroads_10"
           "Crossroads_10_preload"
           "Crossroads_10_boss"
           "Crossroads_10_boss_defeated"
           "Crossroads_11_alt"
           "Crossroads_12"
           "Crossroads_13"
           "Crossroads_14"
           "Crossroads_15"
           "Crossroads_16"
           "Crossroads_18"
           "Crossroads_19"
           "Crossroads_21"
           "Crossroads_22"
           "Crossroads_25"
           "Crossroads_27"
           "Crossroads_30"
           "Crossroads_31"
           "Crossroads_33"
           "Crossroads_35"
           "Crossroads_36"
           "Crossroads_37"
           "Crossroads_38"
           "Crossroads_39"
           "Crossroads_40"
           "Crossroads_42"
           "Crossroads_43"
           "Crossroads_45"
           "Crossroads_46"
           "Crossroads_46b"
           "Crossroads_ShamanTemple"
           "Crossroads_47"
           "Crossroads_48"
           "Crossroads_49"
           "Crossroads_49b"
           "Crossroads_50"
           "Crossroads_52"
           "Ruins_House_01"
           "Ruins_House_02"
           "Ruins_House_03"
           "Ruins_Elevator"
           "Ruins_Bathhouse"
           "Ruins1_01"
           "Ruins1_02"
           "Ruins1_03"
           "Ruins1_04"
           "Ruins1_05"
           "Ruins1_05b"
           "Ruins1_05c"
           "Ruins1_06"
           "Ruins1_09"
           "Ruins1_17"
           "Ruins1_18"
           "Ruins1_23"
           "Ruins1_30"
           "Ruins1_24"
           "Ruins1_24_boss"
           "Ruins1_24_boss_defeated"
           "Ruins1_25"
           "Ruins1_27"
           "Ruins1_28"
           "Ruins1_29"
           "Ruins1_31"
           "Ruins1_31b"
           "Ruins1_32"
           "Ruins2_01"
           "Ruins2_01_b"
           "Ruins2_03"
           "Ruins2_03b"
           "Ruins2_03_boss"
           "Ruins2_04"
           "Ruins2_05"
           "Ruins2_06"
           "Ruins2_07"
           "Ruins2_08"
           "Ruins2_09"
           "Ruins2_10"
           "Ruins2_10b"
           "Ruins2_11"
           "Ruins2_11_b"
           "Ruins2_11_boss"
           "Ruins2_Watcher_Room"
           "Fungus1_01"
           "Fungus1_01b"
           "Fungus1_02"
           "Fungus1_03"
           "Fungus1_04"
           "Fungus1_04_boss"
           "Fungus1_05"
           "Fungus1_06"
           "Fungus1_07"
           "Fungus1_08"
           "Fungus1_09"
           "Fungus1_10"
           "Fungus1_11"
           "Fungus1_12"
           "Fungus1_13"
           "Fungus1_14"
           "Fungus1_15"
           "Fungus1_16_alt"
           "Fungus1_17"
           "Fungus1_19"
           "Fungus1_20_v02"
           "Fungus1_21"
           "Fungus1_22"
           "Fungus1_23"
           "Fungus1_24"
           "Fungus1_25"
           "Fungus1_26"
           "Fungus1_28"
           "Fungus1_29"
           "Fungus1_30"
           "Fungus1_31"
           "Fungus1_32"
           "Fungus1_34"
           "Fungus1_35"
           "Fungus1_36"
           "Fungus1_37"
           "Fungus1_Slug"
           "Fungus2_01"
           "Fungus2_02"
           "Fungus2_03"
           "Fungus2_04"
           "Fungus2_05"
           "Fungus2_06"
           "Fungus2_07"
           "Fungus2_08"
           "Fungus2_09"
           "Fungus2_10"
           "Fungus2_11"
           "Fungus2_12"
           "Fungus2_13"
           "Fungus2_14"
           "Fungus2_15"
           "Fungus2_15_boss"
           "Fungus2_15_boss_defeated"
           "Fungus2_17"
           "Fungus2_18"
           "Fungus2_19"
           "Fungus2_20"
           "Fungus2_21"
           "Fungus2_23"
           "Fungus2_25"
           "Fungus2_26"
           "Fungus2_28"
           "Fungus2_29"
           "Fungus2_30"
           "Fungus2_31"
           "Fungus2_32"
           "Fungus2_33"
           "Fungus2_34"
           "Fungus3_01"
           "Fungus3_02"
           "Fungus3_03"
           "Fungus3_04"
           "Fungus3_05"
           "Fungus3_08"
           "Fungus3_10"
           "Fungus3_11"
           "Fungus3_13"
           "Fungus3_21"
           "Fungus3_22"
           "Fungus3_23"
           "Fungus3_23_boss"
           "Fungus3_24"
           "Fungus3_25"
           "Fungus3_25b"
           "Fungus3_26"
           "Fungus3_27"
           "Fungus3_28"
           "Fungus3_30"
           "Fungus3_34"
           "Fungus3_35"
           "Fungus3_39"
           "Fungus3_40"
           "Fungus3_40_boss"
           "Fungus3_44"
           "Fungus3_47"
           "Fungus3_48"
           "Fungus3_49"
           "Fungus3_50"
           "Fungus3_archive"
           "Fungus3_archive_02"
           "Fungus3_archive_02_boss"
           "Cliffs_01"
           "Cliffs_02"
           "Cliffs_02_boss"
           "Cliffs_03"
           "Cliffs_04"
           "Cliffs_05"
           "Cliffs_06"
           "RestingGrounds_02"
           "RestingGrounds_02_boss"
           "RestingGrounds_04"
           "RestingGrounds_05"
           "RestingGrounds_06"
           "RestingGrounds_07"
           "RestingGrounds_08"
           "RestingGrounds_09"
           "RestingGrounds_10"
           "RestingGrounds_12"
           "RestingGrounds_17"
           "Mines_01"
           "Mines_02"
           "Mines_03"
           "Mines_04"
           "Mines_05"
           "Mines_06"
           "Mines_07"
           "Mines_10"
           "Mines_11"
           "Mines_13"
           "Mines_16"
           "Mines_17"
           "Mines_18"
           "Mines_18_boss"
           "Mines_19"
           "Mines_20"
           "Mines_23"
           "Mines_24"
           "Mines_25"
           "Mines_28"
           "Mines_29"
           "Mines_30"
           "Mines_31"
           "Mines_32"
           "Mines_33"
           "Mines_34"
           "Mines_35"
           "Mines_36"
           "Mines_37"
           "Deepnest_01"
           "Deepnest_01b"
           "Deepnest_02"
           "Deepnest_03"
           "Deepnest_09"
           "Deepnest_10"
           "Deepnest_14"
           "Deepnest_16"
           "Deepnest_17"
           "Deepnest_26"
           "Deepnest_26b"
           "Deepnest_30"
           "Deepnest_31"
           "Deepnest_32"
           "Deepnest_33"
           "Deepnest_34"
           "Deepnest_35"
           "Deepnest_36"
           "Deepnest_37"
           "Deepnest_38"
           "Deepnest_39"
           "Deepnest_40"
           "Deepnest_41"
           "Deepnest_42"
           "Deepnest_43"
           "Deepnest_44"
           "Deepnest_45_v02"
           "Deepnest_Spider_Town"
           "Room_spider_small"
           "Deepnest_East_01"
           "Deepnest_East_02"
           "Deepnest_East_03"
           "Deepnest_East_04"
           "Deepnest_East_06"
           "Deepnest_East_07"
           "Deepnest_East_08"
           "Deepnest_East_09"
           "Deepnest_East_10"
           "Deepnest_East_11"
           "Deepnest_East_12"
           "Deepnest_East_13"
           "Deepnest_East_14"
           "Deepnest_East_14b"
           "Deepnest_East_15"
           "Deepnest_East_16"
           "Deepnest_East_17"
           "Deepnest_East_18"
           "Deepnest_East_Hornet"
           "Deepnest_East_Hornet_boss"
           "Room_Wyrm"
           "Abyss_01"
           "Abyss_02"
           "Abyss_03"
           "Abyss_03_b"
           "Abyss_03_c"
           "Abyss_04"
           "Abyss_05"
           "Abyss_06_Core"
           "Abyss_08"
           "Abyss_09"
           "Abyss_10"
           "Abyss_12"
           "Abyss_15"
           "Abyss_16"
           "Abyss_17"
           "Abyss_18"
           "Abyss_19"
           "Abyss_20"
           "Abyss_21"
           "Abyss_22"
           "Abyss_Lighthouse_room"
           "Room_Queen"
           "Waterways_01"
           "Waterways_02"
           "Waterways_03"
           "Waterways_04"
           "Waterways_04b"
           "Waterways_05"
           "Waterways_05_boss"
           "Waterways_06"
           "Waterways_07"
           "Waterways_08"
           "Waterways_09"
           "Waterways_12"
           "Waterways_12_boss"
           "Waterways_13"
           "Waterways_14"
           "Waterways_15"
           "White_Palace_01"
           "White_Palace_02"
           "White_Palace_03_hub"
           "White_Palace_04"
           "White_Palace_05"
           "White_Palace_06"
           "White_Palace_07"
           "White_Palace_08"
           "White_Palace_09"
           "White_Palace_11"
           "White_Palace_12"
           "White_Palace_13"
           "White_Palace_14"
           "White_Palace_15"
           "White_Palace_16"
           "White_Palace_17"
           "White_Palace_18"
           "White_Palace_19"
           "White_Palace_20"
           "Hive_01"
           "Hive_02"
           "Hive_03"
           "Hive_03_c"
           "Hive_04"
           "Hive_05"
           "Grimm_Divine"
           "Grimm_Main_Tent"
           "Grimm_Main_Tent_boss"
           "Grimm_Nightmare"
           "Dream_Nailcollection"
           "Dream_01_False_Knight"
           "Dream_02_Mage_Lord"
           "Dream_03_Infected_Knight"
           "Dream_04_White_Defender"
           "Dream_Mighty_Zote"
           "Dream_Guardian_Hegemol"
           "Dream_Guardian_Lurien"
           "Dream_Guardian_Monomon"
           "Cutscene_Boss_Door"
           "Dream_Backer_Shrine"
           "Dream_Room_Believer_Shrine"
           "Dream_Abyss"
           "Dream_Final_Boss"
           "Room_Final_Boss_Atrium"
           "Room_Final_Boss_Core"
           "Cinematic_Ending_A"
           "Cinematic_Ending_B"
           "Cinematic_Ending_C"
           "Cinematic_Ending_D"
           "Cinematic_Ending_E"
           "End_Credits"
           "Cinematic_MrMushroom"
           "Menu_Credits"
           "End_Game_Completion"
           "PermaDeath"
           "_test_cocoon_2"
           "PermaDeath_Unlock"
           "_test_cocoon_1"
           "GG_Waterways"
           "GG_Atrium"
           "GG_Broken_Vessel"
           "GG_Brooding_Mawlek"
           "GG_Collector"
           "GG_Crystal_Guardian"
           "GG_Crystal_Guardian_2"
           "GG_Dung_Defender"
           "GG_Failed_Champion"
           "GG_False_Knight"
           "GG_Flukemarm"
           "GG_Ghost_Galien"
           "GG_Ghost_Gorb"
           "GG_Ghost_Hu"
           "GG_Ghost_Markoth"
           "GG_Ghost_Marmu"
           "GG_Ghost_No_Eyes"
           "GG_Ghost_Xero"
           "GG_God_Tamer"
           "GG_Grey_Prince_Zote"
           "GG_Grimm"
           "GG_Grimm_Nightmare"
           "GG_Gruz_Mother"
           "GG_Hive_Knight"
           "GG_Hollow_Knight"
           "GG_Hornet_1"
           "GG_Hornet_2"
           "GG_Lost_Kin"
           "GG_Lurker"
           "GG_Mantis_Lords"
           "GG_Mega_Moss_Charger"
           "GG_Nailmasters"
           "GG_Nosk"
           "GG_Oblobbles"
           "GG_Painter"
           "GG_Pipeway"
           "GG_Radiance"
           "GG_Sly"
           "GG_Soul_Master"
           "GG_Soul_Tyrant"
           "GG_Spa"
           "GG_Traitor_Lord"
           "GG_Unlock"
           "GG_Uumuu"
           "GG_Vengefly"
           "GG_Watcher_Knights"
           "GG_White_Defender"
           "GG_Workshop"
           "Room_GG_Shortcut"
           "GG_End_Sequence"
           "GG_Atrium_Roof"
           "GG_Blue_Room"
           "GG_Engine"
           "GG_Engine_Prime"
           "GG_Engine_Root"
           "GG_Mage_Knight"
           "GG_Vengefly_V"
           "GG_Entrance_Cutscene"
           "GG_Mighty_Zote"
           "GG_Land_of_Storms"
           "GG_Boss_Door_Entrance"
           "GG_Gruz_Mother_V"
           "GG_Brooding_Mawlek_V"
           "GG_Mantis_Lords_V"
           "GG_Nosk_Hornet"
           "GG_Uumuu_V"
           "GG_Ghost_Gorb_V"
           "GG_Ghost_Markoth_V"
           "GG_Ghost_Marmu_V"
           "GG_Ghost_No_Eyes_V"
           "GG_Ghost_Xero_V"
           "GG_Mage_Knight_V"
           "GG_Collector_V"
           "GG_Nosk_V"
           "GG_Wyrm"
           "GG_Unn"
           "GG_Door_5_Finale"
           "GG_Unlock_Wastes" |]

    let sceneIdx s =
        match s with
        | "Pre_Menu_Intro" -> 0
        | "Menu_Title" -> 1
        | "Quit_To_Menu" -> 2
        | "BetaEnd" -> 3
        | "Knight_Pickup" -> 4
        | "Opening_Sequence" -> 5
        | "Tutorial_01" -> 6
        | "Town" -> 7
        | "Cinematic_Stag_travel" -> 8
        | "Room_Town_Stag_Station" -> 9
        | "Room_Charm_Shop" -> 10
        | "Room_Mender_House" -> 11
        | "Room_mapper" -> 12
        | "Room_nailmaster" -> 13
        | "Room_nailmaster_02" -> 14
        | "Room_nailmaster_03" -> 15
        | "Room_nailsmith" -> 16
        | "Room_shop" -> 17
        | "Room_Sly_Storeroom" -> 18
        | "Room_temple" -> 19
        | "Room_ruinhouse" -> 20
        | "Room_Mask_Maker" -> 21
        | "Room_Mansion" -> 22
        | "Room_Tram" -> 23
        | "Room_Tram_RG" -> 24
        | "Room_Bretta" -> 25
        | "Room_Bretta_Basement" -> 26
        | "Room_Fungus_Shaman" -> 27
        | "Room_Ouiji" -> 28
        | "Room_Jinn" -> 29
        | "Room_Colosseum_01" -> 30
        | "Room_Colosseum_02" -> 31
        | "Room_Colosseum_Bronze" -> 32
        | "Room_Colosseum_Silver" -> 33
        | "Room_Colosseum_Gold" -> 34
        | "Room_Colosseum_Spectate" -> 35
        | "Room_Slug_Shrine" -> 36
        | "Crossroads_01" -> 37
        | "Crossroads_02" -> 38
        | "Crossroads_03" -> 39
        | "Crossroads_04" -> 40
        | "Crossroads_05" -> 41
        | "Crossroads_06" -> 42
        | "Crossroads_07" -> 43
        | "Crossroads_08" -> 44
        | "Crossroads_09" -> 45
        | "Crossroads_10" -> 46
        | "Crossroads_10_preload" -> 47
        | "Crossroads_10_boss" -> 48
        | "Crossroads_10_boss_defeated" -> 49
        | "Crossroads_11_alt" -> 50
        | "Crossroads_12" -> 51
        | "Crossroads_13" -> 52
        | "Crossroads_14" -> 53
        | "Crossroads_15" -> 54
        | "Crossroads_16" -> 55
        | "Crossroads_18" -> 56
        | "Crossroads_19" -> 57
        | "Crossroads_21" -> 58
        | "Crossroads_22" -> 59
        | "Crossroads_25" -> 60
        | "Crossroads_27" -> 61
        | "Crossroads_30" -> 62
        | "Crossroads_31" -> 63
        | "Crossroads_33" -> 64
        | "Crossroads_35" -> 65
        | "Crossroads_36" -> 66
        | "Crossroads_37" -> 67
        | "Crossroads_38" -> 68
        | "Crossroads_39" -> 69
        | "Crossroads_40" -> 70
        | "Crossroads_42" -> 71
        | "Crossroads_43" -> 72
        | "Crossroads_45" -> 73
        | "Crossroads_46" -> 74
        | "Crossroads_46b" -> 75
        | "Crossroads_ShamanTemple" -> 76
        | "Crossroads_47" -> 77
        | "Crossroads_48" -> 78
        | "Crossroads_49" -> 79
        | "Crossroads_49b" -> 80
        | "Crossroads_50" -> 81
        | "Crossroads_52" -> 82
        | "Ruins_House_01" -> 83
        | "Ruins_House_02" -> 84
        | "Ruins_House_03" -> 85
        | "Ruins_Elevator" -> 86
        | "Ruins_Bathhouse" -> 87
        | "Ruins1_01" -> 88
        | "Ruins1_02" -> 89
        | "Ruins1_03" -> 90
        | "Ruins1_04" -> 91
        | "Ruins1_05" -> 92
        | "Ruins1_05b" -> 93
        | "Ruins1_05c" -> 94
        | "Ruins1_06" -> 95
        | "Ruins1_09" -> 96
        | "Ruins1_17" -> 97
        | "Ruins1_18" -> 98
        | "Ruins1_23" -> 99
        | "Ruins1_30" -> 100
        | "Ruins1_24" -> 101
        | "Ruins1_24_boss" -> 102
        | "Ruins1_24_boss_defeated" -> 103
        | "Ruins1_25" -> 104
        | "Ruins1_27" -> 105
        | "Ruins1_28" -> 106
        | "Ruins1_29" -> 107
        | "Ruins1_31" -> 108
        | "Ruins1_31b" -> 109
        | "Ruins1_32" -> 110
        | "Ruins2_01" -> 111
        | "Ruins2_01_b" -> 112
        | "Ruins2_03" -> 113
        | "Ruins2_03b" -> 114
        | "Ruins2_03_boss" -> 115
        | "Ruins2_04" -> 116
        | "Ruins2_05" -> 117
        | "Ruins2_06" -> 118
        | "Ruins2_07" -> 119
        | "Ruins2_08" -> 120
        | "Ruins2_09" -> 121
        | "Ruins2_10" -> 122
        | "Ruins2_10b" -> 123
        | "Ruins2_11" -> 124
        | "Ruins2_11_b" -> 125
        | "Ruins2_11_boss" -> 126
        | "Ruins2_Watcher_Room" -> 127
        | "Fungus1_01" -> 128
        | "Fungus1_01b" -> 129
        | "Fungus1_02" -> 130
        | "Fungus1_03" -> 131
        | "Fungus1_04" -> 132
        | "Fungus1_04_boss" -> 133
        | "Fungus1_05" -> 134
        | "Fungus1_06" -> 135
        | "Fungus1_07" -> 136
        | "Fungus1_08" -> 137
        | "Fungus1_09" -> 138
        | "Fungus1_10" -> 139
        | "Fungus1_11" -> 140
        | "Fungus1_12" -> 141
        | "Fungus1_13" -> 142
        | "Fungus1_14" -> 143
        | "Fungus1_15" -> 144
        | "Fungus1_16_alt" -> 145
        | "Fungus1_17" -> 146
        | "Fungus1_19" -> 147
        | "Fungus1_20_v02" -> 148
        | "Fungus1_21" -> 149
        | "Fungus1_22" -> 150
        | "Fungus1_23" -> 151
        | "Fungus1_24" -> 152
        | "Fungus1_25" -> 153
        | "Fungus1_26" -> 154
        | "Fungus1_28" -> 155
        | "Fungus1_29" -> 156
        | "Fungus1_30" -> 157
        | "Fungus1_31" -> 158
        | "Fungus1_32" -> 159
        | "Fungus1_34" -> 160
        | "Fungus1_35" -> 161
        | "Fungus1_36" -> 162
        | "Fungus1_37" -> 163
        | "Fungus1_Slug" -> 164
        | "Fungus2_01" -> 165
        | "Fungus2_02" -> 166
        | "Fungus2_03" -> 167
        | "Fungus2_04" -> 168
        | "Fungus2_05" -> 169
        | "Fungus2_06" -> 170
        | "Fungus2_07" -> 171
        | "Fungus2_08" -> 172
        | "Fungus2_09" -> 173
        | "Fungus2_10" -> 174
        | "Fungus2_11" -> 175
        | "Fungus2_12" -> 176
        | "Fungus2_13" -> 177
        | "Fungus2_14" -> 178
        | "Fungus2_15" -> 179
        | "Fungus2_15_boss" -> 180
        | "Fungus2_15_boss_defeated" -> 181
        | "Fungus2_17" -> 182
        | "Fungus2_18" -> 183
        | "Fungus2_19" -> 184
        | "Fungus2_20" -> 185
        | "Fungus2_21" -> 186
        | "Fungus2_23" -> 187
        | "Fungus2_25" -> 188
        | "Fungus2_26" -> 189
        | "Fungus2_28" -> 190
        | "Fungus2_29" -> 191
        | "Fungus2_30" -> 192
        | "Fungus2_31" -> 193
        | "Fungus2_32" -> 194
        | "Fungus2_33" -> 195
        | "Fungus2_34" -> 196
        | "Fungus3_01" -> 197
        | "Fungus3_02" -> 198
        | "Fungus3_03" -> 199
        | "Fungus3_04" -> 200
        | "Fungus3_05" -> 201
        | "Fungus3_08" -> 202
        | "Fungus3_10" -> 203
        | "Fungus3_11" -> 204
        | "Fungus3_13" -> 205
        | "Fungus3_21" -> 206
        | "Fungus3_22" -> 207
        | "Fungus3_23" -> 208
        | "Fungus3_23_boss" -> 209
        | "Fungus3_24" -> 210
        | "Fungus3_25" -> 211
        | "Fungus3_25b" -> 212
        | "Fungus3_26" -> 213
        | "Fungus3_27" -> 214
        | "Fungus3_28" -> 215
        | "Fungus3_30" -> 216
        | "Fungus3_34" -> 217
        | "Fungus3_35" -> 218
        | "Fungus3_39" -> 219
        | "Fungus3_40" -> 220
        | "Fungus3_40_boss" -> 221
        | "Fungus3_44" -> 222
        | "Fungus3_47" -> 223
        | "Fungus3_48" -> 224
        | "Fungus3_49" -> 225
        | "Fungus3_50" -> 226
        | "Fungus3_archive" -> 227
        | "Fungus3_archive_02" -> 228
        | "Fungus3_archive_02_boss" -> 229
        | "Cliffs_01" -> 230
        | "Cliffs_02" -> 231
        | "Cliffs_02_boss" -> 232
        | "Cliffs_03" -> 233
        | "Cliffs_04" -> 234
        | "Cliffs_05" -> 235
        | "Cliffs_06" -> 236
        | "RestingGrounds_02" -> 237
        | "RestingGrounds_02_boss" -> 238
        | "RestingGrounds_04" -> 239
        | "RestingGrounds_05" -> 240
        | "RestingGrounds_06" -> 241
        | "RestingGrounds_07" -> 242
        | "RestingGrounds_08" -> 243
        | "RestingGrounds_09" -> 244
        | "RestingGrounds_10" -> 245
        | "RestingGrounds_12" -> 246
        | "RestingGrounds_17" -> 247
        | "Mines_01" -> 248
        | "Mines_02" -> 249
        | "Mines_03" -> 250
        | "Mines_04" -> 251
        | "Mines_05" -> 252
        | "Mines_06" -> 253
        | "Mines_07" -> 254
        | "Mines_10" -> 255
        | "Mines_11" -> 256
        | "Mines_13" -> 257
        | "Mines_16" -> 258
        | "Mines_17" -> 259
        | "Mines_18" -> 260
        | "Mines_18_boss" -> 261
        | "Mines_19" -> 262
        | "Mines_20" -> 263
        | "Mines_23" -> 264
        | "Mines_24" -> 265
        | "Mines_25" -> 266
        | "Mines_28" -> 267
        | "Mines_29" -> 268
        | "Mines_30" -> 269
        | "Mines_31" -> 270
        | "Mines_32" -> 271
        | "Mines_33" -> 272
        | "Mines_34" -> 273
        | "Mines_35" -> 274
        | "Mines_36" -> 275
        | "Mines_37" -> 276
        | "Deepnest_01" -> 277
        | "Deepnest_01b" -> 278
        | "Deepnest_02" -> 279
        | "Deepnest_03" -> 280
        | "Deepnest_09" -> 281
        | "Deepnest_10" -> 282
        | "Deepnest_14" -> 283
        | "Deepnest_16" -> 284
        | "Deepnest_17" -> 285
        | "Deepnest_26" -> 286
        | "Deepnest_26b" -> 287
        | "Deepnest_30" -> 288
        | "Deepnest_31" -> 289
        | "Deepnest_32" -> 290
        | "Deepnest_33" -> 291
        | "Deepnest_34" -> 292
        | "Deepnest_35" -> 293
        | "Deepnest_36" -> 294
        | "Deepnest_37" -> 295
        | "Deepnest_38" -> 296
        | "Deepnest_39" -> 297
        | "Deepnest_40" -> 298
        | "Deepnest_41" -> 299
        | "Deepnest_42" -> 300
        | "Deepnest_43" -> 301
        | "Deepnest_44" -> 302
        | "Deepnest_45_v02" -> 303
        | "Deepnest_Spider_Town" -> 304
        | "Room_spider_small" -> 305
        | "Deepnest_East_01" -> 306
        | "Deepnest_East_02" -> 307
        | "Deepnest_East_03" -> 308
        | "Deepnest_East_04" -> 309
        | "Deepnest_East_06" -> 310
        | "Deepnest_East_07" -> 311
        | "Deepnest_East_08" -> 312
        | "Deepnest_East_09" -> 313
        | "Deepnest_East_10" -> 314
        | "Deepnest_East_11" -> 315
        | "Deepnest_East_12" -> 316
        | "Deepnest_East_13" -> 317
        | "Deepnest_East_14" -> 318
        | "Deepnest_East_14b" -> 319
        | "Deepnest_East_15" -> 320
        | "Deepnest_East_16" -> 321
        | "Deepnest_East_17" -> 322
        | "Deepnest_East_18" -> 323
        | "Deepnest_East_Hornet" -> 324
        | "Deepnest_East_Hornet_boss" -> 325
        | "Room_Wyrm" -> 326
        | "Abyss_01" -> 327
        | "Abyss_02" -> 328
        | "Abyss_03" -> 329
        | "Abyss_03_b" -> 330
        | "Abyss_03_c" -> 331
        | "Abyss_04" -> 332
        | "Abyss_05" -> 333
        | "Abyss_06_Core" -> 334
        | "Abyss_08" -> 335
        | "Abyss_09" -> 336
        | "Abyss_10" -> 337
        | "Abyss_12" -> 338
        | "Abyss_15" -> 339
        | "Abyss_16" -> 340
        | "Abyss_17" -> 341
        | "Abyss_18" -> 342
        | "Abyss_19" -> 343
        | "Abyss_20" -> 344
        | "Abyss_21" -> 345
        | "Abyss_22" -> 346
        | "Abyss_Lighthouse_room" -> 347
        | "Room_Queen" -> 348
        | "Waterways_01" -> 349
        | "Waterways_02" -> 350
        | "Waterways_03" -> 351
        | "Waterways_04" -> 352
        | "Waterways_04b" -> 353
        | "Waterways_05" -> 354
        | "Waterways_05_boss" -> 355
        | "Waterways_06" -> 356
        | "Waterways_07" -> 357
        | "Waterways_08" -> 358
        | "Waterways_09" -> 359
        | "Waterways_12" -> 360
        | "Waterways_12_boss" -> 361
        | "Waterways_13" -> 362
        | "Waterways_14" -> 363
        | "Waterways_15" -> 364
        | "White_Palace_01" -> 365
        | "White_Palace_02" -> 366
        | "White_Palace_03_hub" -> 367
        | "White_Palace_04" -> 368
        | "White_Palace_05" -> 369
        | "White_Palace_06" -> 370
        | "White_Palace_07" -> 371
        | "White_Palace_08" -> 372
        | "White_Palace_09" -> 373
        | "White_Palace_11" -> 374
        | "White_Palace_12" -> 375
        | "White_Palace_13" -> 376
        | "White_Palace_14" -> 377
        | "White_Palace_15" -> 378
        | "White_Palace_16" -> 379
        | "White_Palace_17" -> 380
        | "White_Palace_18" -> 381
        | "White_Palace_19" -> 382
        | "White_Palace_20" -> 383
        | "Hive_01" -> 384
        | "Hive_02" -> 385
        | "Hive_03" -> 386
        | "Hive_03_c" -> 387
        | "Hive_04" -> 388
        | "Hive_05" -> 389
        | "Grimm_Divine" -> 390
        | "Grimm_Main_Tent" -> 391
        | "Grimm_Main_Tent_boss" -> 392
        | "Grimm_Nightmare" -> 393
        | "Dream_Nailcollection" -> 394
        | "Dream_01_False_Knight" -> 395
        | "Dream_02_Mage_Lord" -> 396
        | "Dream_03_Infected_Knight" -> 397
        | "Dream_04_White_Defender" -> 398
        | "Dream_Mighty_Zote" -> 399
        | "Dream_Guardian_Hegemol" -> 400
        | "Dream_Guardian_Lurien" -> 401
        | "Dream_Guardian_Monomon" -> 402
        | "Cutscene_Boss_Door" -> 403
        | "Dream_Backer_Shrine" -> 404
        | "Dream_Room_Believer_Shrine" -> 405
        | "Dream_Abyss" -> 406
        | "Dream_Final_Boss" -> 407
        | "Room_Final_Boss_Atrium" -> 408
        | "Room_Final_Boss_Core" -> 409
        | "Cinematic_Ending_A" -> 410
        | "Cinematic_Ending_B" -> 411
        | "Cinematic_Ending_C" -> 412
        | "Cinematic_Ending_D" -> 413
        | "Cinematic_Ending_E" -> 414
        | "End_Credits" -> 415
        | "Cinematic_MrMushroom" -> 416
        | "Menu_Credits" -> 417
        | "End_Game_Completion" -> 418
        | "PermaDeath" -> 419
        | "_test_cocoon_2" -> 420
        | "PermaDeath_Unlock" -> 421
        | "_test_cocoon_1" -> 422
        | "GG_Waterways" -> 423
        | "GG_Atrium" -> 424
        | "GG_Broken_Vessel" -> 425
        | "GG_Brooding_Mawlek" -> 426
        | "GG_Collector" -> 427
        | "GG_Crystal_Guardian" -> 428
        | "GG_Crystal_Guardian_2" -> 429
        | "GG_Dung_Defender" -> 430
        | "GG_Failed_Champion" -> 431
        | "GG_False_Knight" -> 432
        | "GG_Flukemarm" -> 433
        | "GG_Ghost_Galien" -> 434
        | "GG_Ghost_Gorb" -> 435
        | "GG_Ghost_Hu" -> 436
        | "GG_Ghost_Markoth" -> 437
        | "GG_Ghost_Marmu" -> 438
        | "GG_Ghost_No_Eyes" -> 439
        | "GG_Ghost_Xero" -> 440
        | "GG_God_Tamer" -> 441
        | "GG_Grey_Prince_Zote" -> 442
        | "GG_Grimm" -> 443
        | "GG_Grimm_Nightmare" -> 444
        | "GG_Gruz_Mother" -> 445
        | "GG_Hive_Knight" -> 446
        | "GG_Hollow_Knight" -> 447
        | "GG_Hornet_1" -> 448
        | "GG_Hornet_2" -> 449
        | "GG_Lost_Kin" -> 450
        | "GG_Lurker" -> 451
        | "GG_Mantis_Lords" -> 452
        | "GG_Mega_Moss_Charger" -> 453
        | "GG_Nailmasters" -> 454
        | "GG_Nosk" -> 455
        | "GG_Oblobbles" -> 456
        | "GG_Painter" -> 457
        | "GG_Pipeway" -> 458
        | "GG_Radiance" -> 459
        | "GG_Sly" -> 460
        | "GG_Soul_Master" -> 461
        | "GG_Soul_Tyrant" -> 462
        | "GG_Spa" -> 463
        | "GG_Traitor_Lord" -> 464
        | "GG_Unlock" -> 465
        | "GG_Uumuu" -> 466
        | "GG_Vengefly" -> 467
        | "GG_Watcher_Knights" -> 468
        | "GG_White_Defender" -> 469
        | "GG_Workshop" -> 470
        | "Room_GG_Shortcut" -> 471
        | "GG_End_Sequence" -> 472
        | "GG_Atrium_Roof" -> 473
        | "GG_Blue_Room" -> 474
        | "GG_Engine" -> 475
        | "GG_Engine_Prime" -> 476
        | "GG_Engine_Root" -> 477
        | "GG_Mage_Knight" -> 478
        | "GG_Vengefly_V" -> 479
        | "GG_Entrance_Cutscene" -> 480
        | "GG_Mighty_Zote" -> 481
        | "GG_Land_of_Storms" -> 482
        | "GG_Boss_Door_Entrance" -> 483
        | "GG_Gruz_Mother_V" -> 484
        | "GG_Brooding_Mawlek_V" -> 485
        | "GG_Mantis_Lords_V" -> 486
        | "GG_Nosk_Hornet" -> 487
        | "GG_Uumuu_V" -> 488
        | "GG_Ghost_Gorb_V" -> 489
        | "GG_Ghost_Markoth_V" -> 490
        | "GG_Ghost_Marmu_V" -> 491
        | "GG_Ghost_No_Eyes_V" -> 492
        | "GG_Ghost_Xero_V" -> 493
        | "GG_Mage_Knight_V" -> 494
        | "GG_Collector_V" -> 495
        | "GG_Nosk_V" -> 496
        | "GG_Wyrm" -> 497
        | "GG_Unn" -> 498
        | "GG_Door_5_Finale" -> 499
        | "GG_Unlock_Wastes" -> 500
        | _ -> 0

    let sceneDoors s =
        match s with
        | 6 ->
            [ "right1", true, 193.5, 68.0
              "top2", PlayerData.instance.hasWalljump, 11, 80.5 ]
        | 7 ->
            [ "door_jiji", PlayerData.instance.jijiDoorUnlocked, 252.00000610313353, 7.750000007857598
              "door_mapper", true, 154.798609883, 10.713547384000002
              "bot1", true, 185, -1.5
              "left1", PlayerData.instance.hasWalljump, 1.5, 49
              "right1",
              PlayerData.instance.hasWalljump
              && PlayerData.instance.hasDoubleJump
              && PlayerData.instance.hasSuperDash,
              263.5,
              53
              "room_divine", PlayerData.instance.nightmareLanternLit, 72.143166, 10.8265 ]
        | 10 -> [ "left1", true, 11.5, 7 ]
        | 11 -> [ "left1", true, 11.5, 7 ]
        | 12 -> [ "left1", true, 11.5, 7 ]
        | 13 -> [ "left1", true, 11, 5 ]
        | 14 -> [ "left1", true, 10, 5 ]
        | 15 -> [ "left1", true, 10, 5 ]
        | 16 -> [ "left1", true, 8.5, 5.5 ]
        | 17 -> [ "left1", true, 9.5, 7 ]
        | 18 -> [ "top1", true, 16, 16.5 ]
        | 19 -> [ "left1", true, 2, 5 ]
        | 20 -> [ "left1", true, 9.5, 7.5 ]
        | 21 -> [ "right1", true, 60.5, 7 ]
        | 22 -> [ "left1", true, 11, 7 ]
        | 25 -> [ "right1", true, 24.5, 7 ]
        | 26 -> [ "top1", true, 20.5, 15 ]
        | 27 -> [ "left1", true, -0.5, 11 ]
        | 28 -> [ "left1", true, 10, 8 ]
        | 29 -> [ "left1", true, 10, 8 ]
        | 32 -> [ "left1", true, 1.5, 7; "left1 (1)", true, 1.5, 11 ]
        | 33 -> [ "left1", true, 1.5, 7; "left1 (1)", true, 1.5, 11 ]
        | 34 -> [ "left1", true, 1.5, 7; "left1 (1)", true, 1.5, 11 ]
        | 36 -> [ "left1", true, 11.5, 7 ]
        | 37 ->
            [ "left1", true, -0.5, 9
              "right1", true, 100.5, 18
              "top1", true, 52.5, 25.5
              "top2", true, 52.5, 42.5 ]
        | 39 ->
            [ "left1", true, 0.5, 34
              "left2", true, -0.5, 11
              "right1", true, 30.5, 35
              "top1", true, 14, 72.5
              "bot1", true, 15, -0.5
              "right2", true, 30.5, 60 ]
        | 43 ->
            [ "bot1", true, 20, 0.5
              "left1", true, 2.5, 83
              "left2", true, -0.5, 40.5
              "left3", true, -0.5, 20
              "right1", true, 43.5, 83
              "right2", true, 43.5, 44 ]
        | 44 ->
            [ "left1", true, -0.5, 22
              "left2", true, -0.5, 6
              "right1", true, 51.5, 24
              "right2", true, 52.5, 9 ]
        | 50 -> [ "right1", true, 120.5, 13 ]
        | 53 ->
            [ "right1", true, 33.5, 35.5
              "right2", true, 33.5, 7
              "left1", true, -0.5, 34
              "left2", true, -0.5, 11 ]
        | 56 -> [ "right2", true, 41.5, 9; "bot1", true, 21, -0.5; "right1", true, 41.5, 37 ]
        | 57 ->
            [ "left1", true, -0.5, 35
              "left2", true, -0.5, 7
              "right1", true, 50.5, 7
              "top1", true, 18, 45.5 ]
        | 58 ->
            [ "left1", true, -0.5, 6
              "right1", true, 100, 17
              "top1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.hasSuperDash,
              9,
              29.5 ]
        | 59 -> [ "bot1", true, 8, -0.5 ]
        | 61 ->
            [ "bot1", true, 15, -0.5
              "left1", true, -0.5, 66
              "left2", true, -0.5, 39
              "right1", true, 30.5, 21 ]
        | 62 -> [ "left1", true, 1.5, 8 ]
        | 63 -> [ "right1", true, 69.5, 5 ]
        | 64 ->
            [ "left1", PlayerData.instance.crossroadsMawlekWall, -0.5, 37
              "left2", true, -0.5, 10
              "right1", PlayerData.instance.shamanPillar, 45.5, 35
              "right2", true, 45.5, 10.5
              "top1", true, 21, 49.5 ]
        | 67 -> [ "right1", true, 110.5, 4 ]
        | 68 -> [ "right1", true, 68.5, 4 ]
        | 76 -> [ "left1", true, -0.5, 9 ]
        | 78 -> [ "left1", true, -0.5, 4 ]
        | 79 -> [ "left1", true, -0.5, 161; "right1", true, 30.5, 161 ]
        | 80 -> [ "right1", true, 30.5, 6 ]
        | 82 -> [ "left1", true, -0.5, 61 ]
        | 83 -> [ "left1", true, 10.5, 7 ]
        | 84 -> [ "left1", true, 10.5, 7 ]
        | 90 ->
            [ "left1", true, -0.5, 11
              "right2", true, 150.5, 10
              "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 28, 70.5 ]
        | 92 ->
            [ "bot1", true, 20, 102.5
              "top1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump),
              9,
              167.5
              "right2", true, 65.5, 109
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              63.5,
              153.5 ]
        | 93 ->
            [ "right1", true, 65.5, 9
              "bot1", PlayerData.instance.openedWaterwaysManhole, 32, 3
              "left1", true, -0.5, 9
              "top1", true, 29, 35.5 ]
        | 98 -> [ "right2", true, 90.5, 10 ]
        | 99 ->
            [ "top1",
              PlayerData.instance.killedMageKnight
              && PlayerData.instance.brokenMageWindow
              && PlayerData.instance.hasDoubleJump
              && PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              34,
              86.5
              "right2", true, 50.5, 29
              "right1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.killedMageKnight,
              50.5,
              75
              "bot1", true, 38, -0.5
              "left1", true, -0.5, 4 ]
        | 100 ->
            [ "left2", true, -0.5, 4
              "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 41
              "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 80.5, 41
              "bot1", PlayerData.instance.brokenMageWindow, 58, -0.5 ]
        | 101 -> [ "right1", true, 70.5, 43 ]
        | 106 -> [ "right1", true, 120.5, 19; "left1", true, -0.5, 9; "bot1", true, 78, -0.5 ]
        | 108 -> [ "bot1", true, 7, -0.5; "right1", true, 65.5, 4; "left1", true, -0.5, 21 ]
        | 110 -> [ "right2", true, 52.5, 50; "right1", true, 52.5, 66 ]
        | 111 ->
            [ "left2",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              -0.5,
              85
              "top1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              70,
              90.5 ]
        | 116 ->
            [ "left2", true, -0.5, 4
              "door_Ruin_Elevator",
              (PlayerData.instance.hasDash
               || PlayerData.instance.hasWalljump
               || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.bathHouseOpened,
              68.70495,
              32.68663
              "door_Ruin_House_03", PlayerData.instance.city2_sewerDoor, 80.34, 6.62
              "door_Ruin_House_01",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump,
              122.26,
              24.68
              "door_Ruin_House_02",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump,
              83.7,
              39.63
              "right2", true, 150.5, 8
              "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump,
              150.5,
              51 ]
        | 121 -> [ "bot1", true, 8, -0.5 ]
        | 122 -> [ "left1", true, -0.5, 161; "right1", true, 30.5, 161 ]
        | 124 -> [ "right1", true, 48.5, 61 ]
        | 127 -> [ "bot1", true, 53, -0.5 ]
        | 137 -> [ "left1", true, -0.5, 39 ]
        | 140 ->
            [ "right1", true, 55.5, 40
              "bot1", true, 35, -0.5
              "top1", true, 41, 69
              "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDash,
              -0.5,
              33
              "right2", true, 55.5, 62 ]
        | 142 -> [ "right1", true, 220.5, 18; "left1", true, -0.5, 23 ]
        | 143 -> [ "left1", true, -0.5, 13 ]
        | 149 ->
            [ "top1", true, 9, 50.5
              "right1", true, 90.5, 14
              "bot1", true, 21, -0.5
              "left1", true, -0.5, 14 ]
        | 152 -> [ "left1", true, -0.5, 11 ]
        | 157 ->
            [ "top3", true, 94, 32.5
              "top1", true, 28, 32.5
              "right1", true, 100.5, 15
              "left1", true, -0.5, 15 ]
        | 162 -> [ "left1", true, -0.5, 4 ]
        | 163 -> [ "left1", true, -0.5, 7 ]
        | 164 -> [ "right1", true, 70.5, 102 ]
        | 165 ->
            [ "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash,
              60.5,
              42
              "left1", true, -0.5, 42
              "left2", true, -0.5, 6
              "left3", true, -0.5, 19 ]
        | 167 -> [ "right1", true, 130.5, 17; "left1", true, -0.5, 15; "bot1", true, 57, -0.5 ]
        | 168 -> [ "right2", true, 36.5, 55; "left1", true, -0.5, 12 ]
        | 170 ->
            [ "left2", PlayerData.instance.hasAcidArmour, -0.5, 59
              "right2", true, 36.5, 10
              "left1", true, -0.5, 10
              "right1",
              PlayerData.instance.hasAcidArmour
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash,
              36.5,
              59 ]
        | 175 ->
            [ "top1", true, 7, 70.5
              "left1", true, -0.5, 41
              "left2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 15
              "right1", true, 31.5, 5 ]
        | 177 ->
            [ "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 24, 116.5
              "left3",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasAcidArmour,
              -0.5,
              5
              "left2", true, 1.5, 59 ]
        | 178 ->
            [ "bot2", true, 52, -0.5
              "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              35,
              41.5
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasAcidArmour
              || PlayerData.instance.hasSuperDash,
              175.5,
              10
              "bot1", true, 44, -0.5 ]
        | 179 ->
            [ "right1", PlayerData.instance.hasWalljump, 61.5, 88
              "top1", true, 29, 120.5
              "top2", true, 33.5, 120.5
              "top3", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 38, 120.5 ]
        | 182 ->
            [ "left1", PlayerData.instance.hasWalljump, -0.5, 34
              "bot1", true, 8, -0.5
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              45.5,
              37 ]
        | 183 ->
            [ "bot1", PlayerData.instance.hasWalljump, 6, -0.5
              "right1", true, 150.5, 8
              "top1", true, 40, 60.5 ]
        | 185 -> [ "left1", true, -0.5, 31; "right1", true, 63.5, 31 ]
        | 186 ->
            [ "left1", true, -0.5, 11
              "right1", PlayerData.instance.openedCityGate && not PlayerData.instance.cityGateClosed, 150.5, 13 ]
        | 187 ->
            [ "right2", PlayerData.instance.waterwaysGate, 90.5, 27
              "right1", true, 90.5, 63 ]
        | 188 -> [ "right1 (1)", true, 168.5, 10 ]
        | 189 -> [ "left1", true, -0.5, 8 ]
        | 191 -> [ "bot1", true, 8, -0.5; "right1", true, 94.5, 32 ]
        | 192 -> [ "top1", true, 20, 140.5 ]
        | 193 -> [ "left1", true, -0.5, 4 ]
        | 194 -> [ "left1", true, -0.5, 4 ]
        | 196 -> [ "right1", true, 47.5, 7 ]
        | 197 ->
            [ "right2", true, 33.5, 13
              "left1", true, -0.5, 38
              "top1", true, 20, 80.5
              "right1", true, 33.5, 67 ]
        | 198 ->
            [ "right1", PlayerData.instance.oneWayArchive, 30.5, 95
              "left3", true, -0.5, 9
              "left1", true, -0.5, 95
              "right2", true, 31.5, 4
              "left2", true, -0.5, 64 ]
        | 200 -> [ "right2", true, 40.5, 6; "right1", true, 40.5, 57 ]
        | 203 -> [ "bot1", true, 7, -0.5; "top1", true, 49, 44.5 ]
        | 204 -> [ "right1", true, 52.5, 15; "left1", true, -0.5, 62; "left2", true, -0.5, 10 ]
        | 205 ->
            [ "left3", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 36
              "bot1", true, 7, -0.5
              "left2", PlayerData.instance.openedGardensStagStation, -0.5, 6
              "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 60
              "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 30.5, 64 ]
        | 207 ->
            [ "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 31.5, 84
              "left1", true, -0.5, 4
              "bot1", true, 7, -0.5 ]
        | 213 ->
            [ "right1", true, 31.5, 67
              "left1", true, -0.5, 62
              "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 21, 105.5
              "left3", true, -0.5, 10
              "left2", true, -0.5, 38 ]
        | 215 -> [ "right1", true, 75.5, 17 ]
        | 216 -> [ "bot1", true, 32, -0.5 ]
        | 217 ->
            [ "right1", true, 160.5, 14
              "left1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              -0.5,
              12
              "top1", PlayerData.instance.hasDoubleJump || PlayerData.instance.hasWalljump, 80, 26.5 ]
        | 218 -> [ "right1", true, 65.5, 6 ]
        | 219 -> [ "left1", true, -0.5, 48 ]
        | 222 ->
            [ "bot1", PlayerData.instance.hasShadowDash, 53, -0.5
              "right1", true, 95.5, 41 ]
        | 223 -> [ "door1", true, 43.45, 6.77; "right1", true, 80.5, 23; "left1", true, -0.5, 8 ]
        | 225 -> [ "right1", true, 87.5, 9 ]
        | 226 -> [ "right1", true, 40.5, 116 ]
        | 228 -> [ "top1", true, 51, 195.5 ]
        | 230 ->
            [ "right2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 140.5, 92
              "right3", true, 140.5, 8
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              140.5,
              144
              "right4", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 140.5, 32 ]
        | 231 ->
            [ "right1",
              PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash,
              241.5,
              49.5
              "bot2", true, 221.5, -2
              "left1", true, -0.5, 25
              "bot1", true, 188, -0.5
              "left2", true, 3, 54
              "door1", true, 122.54, 5.64 ]
        | 235 -> [ "left1", true, -0.5, 49 ]
        | 236 -> [ "left1", true, -0.5, 5 ]
        | 237 -> [ "bot1", true, 98, 1.5; "left1", true, -0.5, 14; "right1", true, 129.5, 6 ]
        | 240 ->
            [ "left2", true, 2, 79
              "right2", true, 45.5, 55
              "right1", PlayerData.instance.gladeDoorOpened, 33.5, 77
              "left1", true, -0.5, 4
              "bot1", true, 34.5, -0.5
              "left3", true, 1.5, 55 ]
        | 242 -> [ "right1", true, 45, 10 ]
        | 243 -> [ "left1", true, 13.5, 8 ]
        | 245 ->
            [ "top2",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              130.5,
              30.5
              "top1",
              PlayerData.instance.restingGroundsCryptWall
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump),
              72.5,
              30.5
              "left1", true, -0.5, 12 ]
        | 247 -> [ "right1", true, 70.5, 6 ]
        | 248 -> [ "left1", true, -0.5, 49; "bot1", true, 14, -0.5 ]
        | 249 ->
            [ "left1", true, -0.5, 28
              "right1", true, 160.5, 4
              "top2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 119, 36.5
              "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 20, 36.5 ]
        | 250 ->
            [ "bot1", true, 16, 0
              "top1", PlayerData.instance.hasWalljump, 12, 80.5
              "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 45.5, 41 ]
        | 251 ->
            [ "right1", true, 36.5, 56
              "top1", true, 22, 85.5
              "left3", true, -0.5, 7
              "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 67
              "left2", true, -0.5, 45 ]
        | 252 ->
            [ "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 35.5, 33
              "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 15, 74.5
              "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 69
              "bot1", true, 7, -0.5
              "left2", true, -0.5, 22 ]
        | 256 ->
            [ "bot1", true, 13, -0.5
              "right1", true, 43.5, 22
              "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 17, 65.5 ]
        | 258 -> [ "top1", true, 22, 35.5 ]
        | 260 ->
            [ "right1", true, 65.5, 12
              "left1", true, -0.5, 12
              "top1", PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump, 56, 30.5 ]
        | 263 ->
            [ "bot1", true, 39, 59.5
              "right2", true, 75.5, 64
              "right1", PlayerData.instance.hasDoubleJump, 75.5, 196
              "left2", true, -0.5, 120
              "left1", true, -0.5, 187
              "left3", true, -0.5, 64 ]
        | 265 -> [ "left1", true, -0.5, 8 ]
        | 270 -> [ "left1", true, -0.5, 20 ]
        | 271 -> [ "bot1", true, 56, -0.5 ]
        | 273 -> [ "right1", true, 193.5, 50; "bot1", true, 166, -0.5 ]
        | 274 -> [ "left1", true, -0.5, 49 ]
        | 275 -> [ "right1", true, 43.5, 23 ]
        | 277 ->
            [ "right1", true, 60.5, 20
              "left1", true, 1, 21
              "bot2", true, 32, -0.5
              "bot1", true, 9, -0.5 ]
        | 278 ->
            [ "right1", true, 60.5, 66
              "right2",
              PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash,
              60.5,
              37
              "bot1", true, 25, -0.5
              "top1", PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump, 9, 83.5 ]
        | 279 ->
            [ "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 31.5, 45
              "left1", PlayerData.instance.hasWalljump, -0.5, 67
              "left2", true, -0.5, 36 ]
        | 280 ->
            [ "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 48
              "left2", PlayerData.instance.hasSuperDash || PlayerData.instance.hasDoubleJump, -0.5, 15
              "right1", true, 60.5, 4
              "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 48, 113.5 ]
        | 282 ->
            [ "door2",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              19.82,
              94.49
              "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 75.5, 139
              "right2", true, 75.5, 89
              "right3", true, 75.5, 14 ]
        | 283 ->
            [ "bot1", true, 14, -0.5
              "bot2", true, 63, -0.5
              "left1",
              PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              6.5,
              42
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              80.5,
              4
              "left1 (1)", true, 6.5, 46
              "left1 (2)", true, 6.5, 50
              "left1 (3)", true, 6.5, 54 ]
        | 284 -> [ "bot1", true, 73, -0.5 ]
        | 285 ->
            [ "bot1", true, 14, -0.5
              "left1", true, -0.5, 8
              "right1", true, 31.5, 5
              "top1", true, 16, 62.5 ]
        | 286 -> [ "bot1", true, 224, -0.5; "right1", true, 246, 18 ]
        | 288 -> [ "top1", true, 20, 159.5 ]
        | 290 -> [ "left1", true, -0.5, 5 ]
        | 291 -> [ "top1", true, 14, 45.5 ]
        | 292 ->
            [ "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 16
              "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
                 && (PlayerData.instance.hasWalljump
                     || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash),
              98,
              50.5
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              150.5,
              28 ]
        | 293 ->
            [ "left1", true, -0.5, 53
              "bot1", true, 17, -0.5
              "top1", PlayerData.instance.hasWalljump, 35, 110.5 ]
        | 294 -> [ "left1", true, -0.5, 16 ]
        | 295 ->
            [ "bot1", true, 44, -0.5
              "top1", true, 19, 32.5
              "left1", true, -0.5, 4
              "right1", true, 82.5, 4 ]
        | 296 -> [ "bot1", true, 8, -0.5 ]
        | 298 -> [ "right1", true, 150.5, 17 ]
        | 299 -> [ "left2", true, -0.5, 6 ]
        | 300 ->
            [ "top1", PlayerData.instance.hasDoubleJump, 7, 146.5
              "bot1", true, 26, -0.5
              "left1", true, -0.5, 124 ]
        | 302 -> [ "top1", true, 39, 60.5 ]
        | 303 -> [ "left1", true, 18.5, 16 ]
        | 304 -> [ "left1", true, 5.5, 59 ]
        | 305 -> [ "left1", true, 12.5, 14 ]
        | 307 ->
            [ "bot1", true, 12, -0.5
              "bot2", true, 71, -0.5
              "right1", true, 110.5, 17
              "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 16, 32.5 ]
        | 308 ->
            [ "top1", PlayerData.instance.hasWalljump, 9, 150.5
              "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              80.5,
              132
              "right2", true, 80.5, 17
              "left2", PlayerData.instance.outskirtsWall, -0.5, 18
              "left1", true, -0.5, 101 ]
        | 309 ->
            [ "right2",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.hasDoubleJump,
              45.5,
              127
              "left2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 94
              "right1", true, 45.5, 7
              "left1", PlayerData.instance.hasAcidArmour, -0.5, 7 ]
        | 311 ->
            [ "left2", true, 1.5, 32
              "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, -0.5, 171
              "right1", true, 90.5, 65
              "bot2", true, 37, -0.5
              "bot1", true, 6, -0.5 ]
        | 314 -> [ "left1", true, -0.5, 4 ]
        | 317 -> [ "bot1", true, 19.5, -0.5 ]
        | 318 -> [ "top2", true, 150, 70.5 ]
        | 320 -> [ "left1", true, -0.5, 5 ]
        | 321 -> [ "bot1", true, 42, 1.5 ]
        | 322 -> [ "left1", true, 19, 294 ]
        | 323 ->
            [ "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 37, 41.5
              "right2",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.hasShadowDash,
              110.5,
              15
              "bot1", true, 65, -0.5 ]
        | 324 -> [ "left1", true, -0.5, 83 ]
        | 326 -> [ "right1", true, 108.5, 12 ]
        | 327 ->
            [ "left1",
              PlayerData.instance.dungDefenderWallBroken
              && (PlayerData.instance.hasWalljump
                  && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
                  || PlayerData.instance.hasDoubleJump),
              -0.5,
              133
              "right1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDoubleJump || PlayerData.instance.hasDash),
              30.5,
              161
              "left3", true, -0.5, 11 ]
        | 329 -> [ "top1", true, 47, 22.5; "bot1", true, 14, 0; "bot2", true, 62.5, 7 ]
        | 330 -> [ "left1", true, -0.5, 10 ]
        | 331 ->
            [ "top1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 140, 25.5
              "right1", true, 150.5, 10 ]
        | 332 ->
            [ "bot1", true, 59, -0.5
              "top1", PlayerData.instance.hasWalljump, 56, 90.5
              "right1", PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump, 100.5, 21
              "left1", true, -0.5, 9 ]
        | 333 -> [ "right1", true, 200.5, 18; "left1", true, -0.5, 18 ]
        | 334 ->
            [ "bot1", true, 27, -0.5
              "left1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              0.5,
              140
              "left1 extra", true, 0.5, 144
              "left3", true, -0.5, 6
              "right2", true, 100.5, 6
              "top1", true, 90, 270.5 ]
        | 335 -> [ "right1", true, 110, 92 ]
        | 336 ->
            [ "right3", true, 260.5, 57
              "right1", PlayerData.instance.hasAcidArmour || PlayerData.instance.hasSuperDash, 260.5, 26
              "left1", true, -0.5, 26
              "right2",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              81.50000081923,
              90.9999970592716 ]
        | 338 -> [ "right1", true, 180.5, 16 ]
        | 339 -> [ "top1", true, 11, 110.5 ]
        | 341 -> [ "top1", true, 163, 37 ]
        | 343 -> [ "bot1", true, 84, -0.5 ]
        | 345 -> [ "right1", true, 170.5, 247 ]
        | 347 -> [ "left1", true, 8, 8 ]
        | 348 -> [ "left1", true, 4.5, 45 ]
        | 349 ->
            [ "top1",
              PlayerData.instance.openedWaterwaysManhole
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasWalljump
                     && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)),
              77,
              47
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              152.5,
              41
              "bot1", true, 54, -0.5
              "left1", true, -0.5, 14 ]
        | 350 ->
            [ "top3", true, 10, 45.5
              "bot1", true, 9, -0.5
              "top2",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              171,
              45.5
              "top1", PlayerData.instance.hasWalljump, 65, 45.5 ]
        | 351 -> [ "left1", true, 44, 5 ]
        | 352 ->
            [ "bot1", true, 146, -0.5
              "left2", PlayerData.instance.hasAcidArmour, -0.5, 9
              "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash
                     || PlayerData.instance.hasSuperDash
                     || PlayerData.instance.hasDoubleJump),
              -0.5,
              34
              "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 154.5, 37 ]
        | 354 -> [ "bot2", true, 104, 3; "bot1", true, 8, -0.5 ]
        | 357 ->
            [ "top1", true, 11, 100.5
              "right1", PlayerData.instance.hasWalljump && PlayerData.instance.waterwaysAcidDrained, 110.5, 50
              "left1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasWalljump
                 && PlayerData.instance.hasAcidArmour
                 && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
                 && PlayerData.instance.hasAcidArmour
                 && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasWalljump
                 && PlayerData.instance.hasDash
                 && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
                 && PlayerData.instance.hasDoubleJump
                 && PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump
                 && PlayerData.instance.hasDoubleJump
                 && PlayerData.instance.hasSuperDash,
              -0.5,
              21
              "door1", PlayerData.instance.hasWalljump, 81.11, 86.68
              "right2", PlayerData.instance.hasAcidArmour, 110.5, 17 ]
        | 360 -> [ "right1", true, 80.5, 6 ]
        | 362 ->
            [ "left2", PlayerData.instance.hasAcidArmour, -0.5, 17
              "left1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.hasAcidArmour,
              -0.5,
              48 ]
        | 364 -> [ "top1", true, 39, 25 ]
        | 365 ->
            [ "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump,
              200.5,
              24
              "left1", true, 0, 21
              "top1",
              (PlayerData.instance.hasDash
               || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
               || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.whitePalaceOrb_1
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump),
              107,
              140 ]
        | 366 -> [ "left1", true, -0.5, 24 ]
        | 367 ->
            [ "top1",
              PlayerData.instance.whitePalaceOrb_2
              && PlayerData.instance.whitePalaceOrb_3
              && (PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash),
              54,
              140.5
              "bot1", true, 10, -0.5
              "right1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasWalljump)
              || PlayerData.instance.hasDoubleJump,
              100.5,
              64
              "left1", true, -0.5, 98
              "left2", true, -0.5, 35 ]
        | 373 -> [ "right1", true, 150.5, 10 ]
        | 374 -> [ "door2", true, 128.81, 16.8 ]
        | 375 -> [ "bot1", true, 34, 132 ]
        | 376 ->
            [ "right1",
              PlayerData.instance.hasSuperDash
              && (PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasDash && PlayerData.instance.hasWalljump),
              175.5,
              151
              "left3", PlayerData.instance.hasDoubleJump, 98.5, 215
              "left1", PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump, -0.5, 231
              "left2",
              PlayerData.instance.hasWalljump
              && PlayerData.instance.hasDoubleJump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasWalljump
                 && PlayerData.instance.hasDash
                 && PlayerData.instance.hasSuperDash,
              -0.5,
              136 ]
        | 378 -> [ "right1", true, 70.5, 115 ]
        | 380 -> [ "bot1", true, 80, -0.5 ]
        | 383 -> [ "bot1", true, 10, 154 ]
        | 384 -> [ "right2", true, 120.5, 9 ]
        | 386 -> [ "top1", true, 41, 150.5 ]
        | 389 -> [ "left1", true, -0.5, 28 ]
        | 390 -> [ "left1", true, 11.5, 7 ]
        | 391 -> [ "left1", true, 11, 7 ]
        | 405 -> [ "left1", true, 5, 17 ]
        | 408 -> [ "right1", true, 246, 9; "left1", true, 6.5, 9 ]
        | _ -> []

    let doorDoors s d =
        match s, d with
        | 7, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasSuperDash,
              262.03053257206494,
              Dir.W,
              1.5,
              49.0 ]
        | 7, "left1" ->
            [ "right1",
              PlayerData.instance.hasDoubleJump && PlayerData.instance.hasSuperDash,
              262.03053257206494,
              Dir.E,
              263.5,
              53 ]
        | 9, "door_stagExit" -> [ "left1", true, 36.03105882429769, Dir.Nww, 23, 21 ]
        | 30, "bot1" -> [ "left1", true, 31.084562084739105, Dir.W, 13.5, 9 ]
        | 30, "left1" -> [ "bot1", true, 31.084562084739105, Dir.E, 44, 3 ]
        | 31, "top2" -> [ "top1", PlayerData.instance.hasWalljump, 92.0, Dir.W, 43.5, 56 ]
        | 31, "top1" -> [ "top2", PlayerData.instance.hasWalljump, 92.0, Dir.E, 135.5, 56 ]
        | 35, "right1" -> [ "bot1", true, 41.512046444375635, Dir.W, 135, 2.5 ]
        | 37, "right1" ->
            [ "left1", true, 101.40019723846694, Dir.W, -0.5, 9
              "top1", true, 48.58240422210494, Dir.W, 52.5, 25.5 ]
        | 37, "top1" ->
            [ "left1", true, 55.509008277936296, Dir.Sww, -0.5, 9
              "right1", true, 48.58240422210494, Dir.E, 100.5, 18 ]
        | 37, "left1" ->
            [ "right1", true, 101.40019723846694, Dir.E, 100.5, 18
              "top1", true, 55.509008277936296, Dir.Nee, 52.5, 25.5 ]
        | 38, "door1" ->
            [ "left1", true, 47.08092819815684, Dir.W, -0.5, 7
              "right1", true, 44.04717698105067, Dir.E, 90.5, 5 ]
        | 38, "right1" -> [ "left1", true, 91.02197536858887, Dir.W, -0.5, 7 ]
        | 38, "left1" -> [ "right1", true, 91.02197536858887, Dir.E, 90.5, 5 ]
        | 40, "door_Mender_House" ->
            [ "left1", true, 59.35639887806423, Dir.Nww, -0.5, 17
              "top1", true, 33.643148920314516, Dir.Ne, 76.0, 30.5
              "door1", true, 27.950421036807228, Dir.E, 85.050003, 2.82
              "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              105.78423422906354,
              Dir.Nee,
              160.500003,
              25 ]
        | 40, "door1" ->
            [ "left1", true, 86.71721520724711, Dir.W, -0.5, 17
              "top1", true, 29.121898191910656, Dir.Nnw, 76.0, 30.5
              "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              78.64257689063857,
              Dir.Nee,
              160.500003,
              25 ]
        | 40, "door_charmshop" ->
            [ "left1", true, 144.6954754709352, Dir.W, -0.5, 17
              "top1", true, 70.87045172538414, Dir.Nww, 76.0, 30.5
              "door1", true, 59.53911243795985, Dir.W, 85.050003, 2.82
              "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              21.76284366529394,
              Dir.Ne,
              160.500003,
              25 ]
        | 40, "right1" ->
            [ "left1", true, 161.198638226258, Dir.W, -0.5, 17
              "top1", true, 84.67880789784424, Dir.W, 76.0, 30.5
              "door1", true, 78.64257689063857, Dir.Sww, 85.050003, 2.82 ]
        | 40, "top1" ->
            [ "left1", true, 77.68204425734432, Dir.W, -0.5, 17
              "door1", PlayerData.instance.killedBigFly, 29.121898191910656, Dir.Sse, 85.050003, 2.82
              "right1",
              PlayerData.instance.killedBigFly
              && (PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash),
              84.67880789784424,
              Dir.E,
              160.500003,
              25 ]
        | 40, "left1" ->
            [ "top1", true, 77.68204425734432, Dir.E, 76.0, 30.5
              "door1", PlayerData.instance.killedBigFly, 86.71721520724711, Dir.E, 85.050003, 2.82
              "right1",
              PlayerData.instance.killedBigFly
              && (PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash),
              161.198638226258,
              Dir.E,
              160.500003,
              25 ]
        | 41, "right1" -> [ "left1", true, 75.50662222613325, Dir.W, -0.5, 7 ]
        | 41, "left1" -> [ "right1", true, 75.50662222613325, Dir.E, 75, 8 ]
        | 42, "door1" ->
            [ "left1", true, 37.35751196212082, Dir.Sw, -0.5, 6
              "right1", true, 39.86933282612088, Dir.Se, 60.5, 6 ]
        | 42, "right1" -> [ "left1", true, 61.0, Dir.W, -0.5, 6 ]
        | 42, "left1" -> [ "right1", true, 61.0, Dir.E, 60.5, 6 ]
        | 45, "right1" -> [ "left1", PlayerData.instance.killedMawlek, 87.09190547921202, Dir.W, -0.5, 9 ]
        | 45, "left1" -> [ "right1", PlayerData.instance.killedMawlek, 87.09190547921202, Dir.E, 86.5, 5 ]
        | 46, "right1" -> [ "left1", PlayerData.instance.killedFalseKnight, 76.0, Dir.W, -0.5, 4 ]
        | 46, "left1" -> [ "right1", PlayerData.instance.killedFalseKnight, 76.0, Dir.E, 75.5, 4 ]
        | 50, "right1" -> [ "left1", PlayerData.instance.blocker2Defeated, 121.1486689980538, Dir.W, -0.5, 19 ]
        | 51, "right1" -> [ "left1", true, 71.02816342831905, Dir.W, -0.5, 13 ]
        | 51, "left1" -> [ "right1", true, 71.02816342831905, Dir.E, 70.5, 11 ]
        | 52, "right1" -> [ "left1", true, 81.00617260431454, Dir.W, -0.5, 13 ]
        | 52, "left1" -> [ "right1", true, 81.00617260431454, Dir.E, 80.5, 14 ]
        | 54, "right1" -> [ "left1", true, 61.0, Dir.W, -0.5, 4 ]
        | 54, "left1" -> [ "right1", true, 61.0, Dir.E, 60.5, 4 ]
        | 55, "right1" ->
            [ "bot1", true, 24.13503677229434, Dir.Sw, 58, -0.5
              "left1", true, 77.1621668954417, Dir.W, -0.5, 10 ]
        | 55, "left1" ->
            [ "bot1", true, 59.434838268476845, Dir.E, 58, -0.5
              "right1", true, 77.1621668954417, Dir.E, 76.5, 15 ]
        | 55, "bot1" ->
            [ "left1", true, 59.434838268476845, Dir.W, -0.5, 10
              "right1", true, 24.13503677229434, Dir.Ne, 76.5, 15 ]
        | 60, "right1" -> [ "left1", true, 71.0, Dir.W, -0.5, 8 ]
        | 60, "left1" -> [ "right1", true, 71.0, Dir.E, 70.5, 8 ]
        | 65, "bot1" ->
            [ "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasAcidArmour,
              49.924943665466465,
              Dir.N,
              70.5,
              49 ]
        | 65, "right1" -> [ "bot1", PlayerData.instance.hasAcidArmour, 49.924943665466465, Dir.S, 64, -0.5 ]
        | 66, "right2" -> [ "right1", true, 40.0, Dir.N, 60.5, 45 ]
        | 66, "right1" -> [ "right2", true, 40.0, Dir.S, 60.5, 5 ]
        | 69, "right1" -> [ "left1", true, 89.0, Dir.W, -0.5, 7 ]
        | 69, "left1" -> [ "right1", true, 89.0, Dir.E, 88.5, 7 ]
        | 70, "right1" -> [ "left1", true, 89.00140448330015, Dir.W, -0.5, 4.5 ]
        | 70, "left1" -> [ "right1", true, 89.00140448330015, Dir.E, 88.5, 4 ]
        | 71, "right1" -> [ "left1", true, 111.00450441310929, Dir.W, -0.5, 4 ]
        | 71, "left1" -> [ "right1", true, 111.00450441310929, Dir.E, 110.5, 5 ]
        | 72, "right1" -> [ "left1", true, 89.20201791439474, Dir.W, -0.5, 10 ]
        | 72, "left1" -> [ "right1", true, 89.20201791439474, Dir.E, 88.5, 4 ]
        | 73, "right1" -> [ "left1", true, 77.87810988975015, Dir.Sww, -0.5, 9 ]
        | 73, "left1" -> [ "right1", true, 77.87810988975015, Dir.Nee, 70.5, 41 ]
        | 74, "door_tram" -> [ "left1", true, 18.97689647966706, Dir.W, -0.5, 11 ]
        | 75, "door_tram" -> [ "right1", true, 23.102088650163218, Dir.E, 55.5, 11 ]
        | 77, "door_stagExit" -> [ "right1", true, 31.3253507562166, Dir.E, 47.5, 7 ]
        | 81, "left1" ->
            [ "right1",
              PlayerData.instance.hasSuperDash || PlayerData.instance.hasAcidArmour,
              261.5530538915575,
              Dir.E,
              260.5,
              28 ]
        | 81, "right1" ->
            [ "left1",
              (PlayerData.instance.hasSuperDash || PlayerData.instance.hasAcidArmour)
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              261.5530538915575,
              Dir.W,
              -0.5,
              45 ]
        | 85, "left1" -> [ "left2", true, 54.45410911951457, Dir.Sse, 38, 5 ]
        | 85, "left2" -> [ "left1", PlayerData.instance.hasWalljump, 54.45410911951457, Dir.Nnw, 10.5, 52 ]
        | 86, "left1" -> [ "left2", true, 134.4107510580906, Dir.N, 21, 141 ]
        | 86, "left2" -> [ "left1", true, 134.4107510580906, Dir.S, 10.5, 7 ]
        | 87, "door1" -> [ "right1", true, 74.72928876953132, Dir.Nee, 80.5, 33 ]
        | 87, "right1" -> [ "door1", true, 74.72928876953132, Dir.Sww, 11.75, 3.71 ]
        | 88, "bot1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              56.08029957123981,
              Dir.Nw,
              52,
              35.5
              "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              97.2753822917186,
              Dir.W,
              -0.5,
              18 ]
        | 88, "left1" ->
            [ "top1", true, 55.339859052946636, Dir.Nee, 52, 35.5
              "bot1", true, 97.2753822917186, Dir.E, 95, -0.5 ]
        | 88, "top1" ->
            [ "left1", true, 55.339859052946636, Dir.Sww, -0.5, 18
              "bot1", true, 56.08029957123981, Dir.Se, 95, -0.5 ]
        | 89, "bot1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              77.10382610480494,
              Dir.N,
              11,
              76.5 ]
        | 89, "top1" -> [ "bot1", true, 77.10382610480494, Dir.S, 7, -0.5 ]
        | 91, "door1" -> [ "right1", true, 115.21041477460967, Dir.See, 150.5, 9 ]
        | 91, "bot1" ->
            [ "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump,
              135.83261758502633,
              Dir.E,
              150.5,
              9 ]
        | 92, "right1" ->
            [ "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              56.26944108483752,
              Dir.Nww,
              9,
              167.5 ]
        | 92, "top1" -> [ "right1", true, 56.26944108483752, Dir.See, 63.5, 153.5 ]
        | 95, "right1" -> [ "left1", true, 101.0, Dir.W, -0.5, 19 ]
        | 95, "left1" -> [ "right1", true, 101.0, Dir.E, 100.5, 19 ]
        | 96, "left1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              31.784430150625635,
              Dir.Nee,
              31,
              29.5 ]
        | 96, "top1" -> [ "left1", true, 31.784430150625635, Dir.Sww, 1, 19 ]
        | 97, "right1" ->
            [ "top1", true, 106.74502330319667, Dir.Nw, 7, 70.5
              "bot1", true, 83.6211695684771, Dir.W, 7, -0.5 ]
        | 97, "top1" ->
            [ "right1", true, 106.74502330319667, Dir.Se, 90.5, 4
              "bot1", true, 71.0, Dir.S, 7, -0.5 ]
        | 98, "right1" -> [ "left1", true, 91.0, Dir.W, -0.5, 23 ]
        | 99, "top1" -> [ "right1", PlayerData.instance.killedMageKnight, 20.112185361118765, Dir.Se, 50.5, 75 ]
        | 100, "left1" -> [ "right1", true, 81.0, Dir.E, 80.5, 41 ]
        | 101, "right2" -> [ "left2", true, 71.17583859709698, Dir.W, -0.5, 4 ]
        | 101, "right1" ->
            [ "left1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasDoubleJump
               || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash)
              && PlayerData.instance.mageLordDefeated,
              74.94664769020693,
              Dir.Sww,
              -0.5,
              19 ]
        | 101, "left2" -> [ "right2", true, 71.17583859709698, Dir.E, 70.5, 9 ]
        | 104, "left2" ->
            [ "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 50.5, Dir.N, -0.5, 99.5
              "left3", true, 45.0, Dir.S, -0.5, 4 ]
        | 104, "left3" ->
            [ "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 95.5, Dir.N, -0.5, 99.5
              "left2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 45.0, Dir.N, -0.5, 49 ]
        | 104, "left1" ->
            [ "left3", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 95.5, Dir.S, -0.5, 4
              "left2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 50.5, Dir.S, -0.5, 49 ]
        | 105, "right1" -> [ "left1", true, 81.02468759581859, Dir.W, -0.5, 8 ]
        | 107, "door_stagExit" -> [ "left1", true, 32.35454373036344, Dir.W, -0.5, 7 ]
        | 108, "left1" -> [ "bot1", true, 22.7705950734714, Dir.Sse, 7, -0.5 ]
        | 108, "bot1" -> [ "left1", true, 22.7705950734714, Dir.Nnw, -0.5, 21 ]
        | 111, "top1" -> [ "left2", true, 70.71421356417676, Dir.W, -0.5, 85 ]
        | 111, "left2" ->
            [ "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              70.71421356417676,
              Dir.E,
              70,
              90.5 ]
        | 113, "bot1" ->
            [ "top1",
              PlayerData.instance.hasWalljump && PlayerData.instance.killedBlackKnight,
              97.73944955850733,
              Dir.Nne,
              52,
              124.5 ]
        | 113, "top1" -> [ "bot1", PlayerData.instance.killedBlackKnight, 97.73944955850733, Dir.Ssw, 19, 32.5 ]
        | 116, "door_Ruin_House_02" ->
            [ "door_Ruin_Elevator", PlayerData.instance.bathHouseOpened, 16.52458506164074, Dir.Sww, 68.70495, 32.68663
              "door_Ruin_House_01", true, 41.35669353321177, Dir.See, 122.26, 24.68
              "right1", true, 67.7607327292142, Dir.E, 150.5, 51 ]
        | 116, "right1" ->
            [ "door_Ruin_Elevator", PlayerData.instance.bathHouseOpened, 83.82010334794035, Dir.Sww, 68.70495, 32.68663
              "door_Ruin_House_01", true, 38.603626772623315, Dir.Sw, 122.26, 24.68
              "door_Ruin_House_02", true, 67.7607327292142, Dir.W, 83.7, 39.63 ]
        | 116, "door_Ruin_Elevator" ->
            [ "door_Ruin_House_01",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              54.150249348081495,
              Dir.E,
              122.26,
              24.68
              "door_Ruin_House_02",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              16.52458506164074,
              Dir.Nee,
              83.7,
              39.63
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              83.82010334794035,
              Dir.Nee,
              150.5,
              51 ]
        | 117, "bot1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              85.00588214941364,
              Dir.N,
              10,
              84.5
              "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              58.27949896833363,
              Dir.N,
              -0.5,
              57 ]
        | 117, "left1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              29.436372059070052,
              Dir.Nne,
              10,
              84.5
              "bot1", true, 58.27949896833363, Dir.S, 9, -0.5 ]
        | 117, "top1" ->
            [ "bot1", true, 85.00588214941364, Dir.S, 9, -0.5
              "left1", true, 29.436372059070052, Dir.Ssw, -0.5, 57 ]
        | 118, "top1" ->
            [ "right2", true, 54.20332093147061, Dir.Se, 55.5, 6
              "left2",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasAcidArmour,
              48.76474136094644,
              Dir.Ssw,
              -0.5,
              6
              "right1", true, 37.12142238654117, Dir.See, 55.5, 32
              "left1", true, 24.041630560342615, Dir.Sww, -0.5, 42 ]
        | 118, "right1" ->
            [ "right2", true, 26.0, Dir.S, 55.5, 6
              "left2",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasAcidArmour,
              61.741396161732524,
              Dir.Sww,
              -0.5,
              6
              "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              56.88585061331157,
              Dir.W,
              -0.5,
              42
              "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              37.12142238654117,
              Dir.Nww,
              22.5,
              49 ]
        | 118, "left2" ->
            [ "right2",
              PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasAcidArmour,
              56.0,
              Dir.E,
              55.5,
              6
              "right1",
              (PlayerData.instance.hasDoubleJump
               || PlayerData.instance.hasSuperDash
               || PlayerData.instance.hasAcidArmour)
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              61.741396161732524,
              Dir.Nee,
              55.5,
              32
              "left1",
              (PlayerData.instance.hasDoubleJump
               || PlayerData.instance.hasSuperDash
               || PlayerData.instance.hasAcidArmour)
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              36.0,
              Dir.N,
              -0.5,
              42
              "top1",
              (PlayerData.instance.hasDoubleJump
               || PlayerData.instance.hasSuperDash
               || PlayerData.instance.hasAcidArmour)
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              48.76474136094644,
              Dir.Nne,
              22.5,
              49 ]
        | 118, "left1" ->
            [ "right2", true, 66.57326790837296, Dir.See, 55.5, 6
              "left2",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasAcidArmour,
              36.0,
              Dir.S,
              -0.5,
              6
              "right1", true, 56.88585061331157, Dir.E, 55.5, 32
              "top1", true, 24.041630560342615, Dir.Nee, 22.5, 49 ]
        | 118, "right2" ->
            [ "left2",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasAcidArmour,
              56.0,
              Dir.W,
              -0.5,
              6
              "right1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 26.0, Dir.N, 55.5, 32
              "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              66.57326790837296,
              Dir.Nww,
              -0.5,
              42
              "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              54.20332093147061,
              Dir.Nw,
              22.5,
              49 ]
        | 119, "left1" -> [ "right1", PlayerData.instance.hasAcidArmour, 131.03434664239754, Dir.E, 130.5, 10 ]
        | 119, "top1" ->
            [ "right1", true, 30.37268509697488, Dir.See, 130.5, 10
              "left1", PlayerData.instance.hasAcidArmour, 103.38520203588132, Dir.W, -0.5, 7 ]
        | 119, "right1" -> [ "left1", PlayerData.instance.hasAcidArmour, 131.03434664239754, Dir.W, -0.5, 7 ]
        | 120, "door_stagExit" -> [ "left1", true, 32.37857470612318, Dir.W, -0.5, 7 ]
        | 123, "right2" ->
            [ "left1",
              (PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
               || PlayerData.instance.hasWalljump)
              && PlayerData.instance.bathHouseWall,
              87.66413177577246,
              Dir.Nnw,
              -0.5,
              93
              "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump,
              129.0,
              Dir.N,
              30.5,
              140 ]
        | 123, "right1" ->
            [ "left1", PlayerData.instance.bathHouseWall, 56.302753041036986, Dir.Ssw, -0.5, 93
              "right2", true, 129.0, Dir.S, 30.5, 11 ]
        | 123, "left1" ->
            [ "right1",
              PlayerData.instance.cityLift2
              && (PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump)
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              56.302753041036986,
              Dir.Nne,
              30.5,
              140
              "right2", true, 87.66413177577246, Dir.Sse, 30.5, 11 ]
        | 128, "right1" -> [ "left1", true, 171.01827387738422, Dir.W, -0.5, 7 ]
        | 128, "left1" -> [ "right1", true, 171.01827387738422, Dir.E, 170.5, 9.5 ]
        | 129, "left1" -> [ "right1", true, 46.0, Dir.E, 45.5, 7 ]
        | 129, "right1" -> [ "left1", true, 46.0, Dir.W, -0.5, 7 ]
        | 130, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash,
              47.0,
              Dir.W,
              -0.5,
              62
              "right2", true, 51.0, Dir.S, 46.5, 11 ]
        | 130, "right2" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash,
              69.35416353759881,
              Dir.Nw,
              -0.5,
              62
              "right1", true, 51.0, Dir.N, 46.5, 62 ]
        | 130, "left1" ->
            [ "right1", true, 47.0, Dir.E, 46.5, 62
              "right2", true, 69.35416353759881, Dir.Se, 46.5, 11 ]
        | 131, "right1" ->
            [ "bot1", true, 40.03123780249619, Dir.Sww, 45, -0.5
              "left1", true, 81.88406438373708, Dir.W, -0.5, 30 ]
        | 131, "left1" ->
            [ "bot1", true, 54.77681991499689, Dir.Se, 45, -0.5
              "right1", true, 81.88406438373708, Dir.E, 80.5, 18 ]
        | 131, "bot1" ->
            [ "right1", true, 40.03123780249619, Dir.Nee, 80.5, 18
              "left1", true, 54.77681991499689, Dir.Nw, -0.5, 30 ]
        | 132, "left1" ->
            [ "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasAcidArmour || PlayerData.instance.hasDash),
              99.72462083156798,
              Dir.Nee,
              95.5,
              33 ]
        | 132, "right1" ->
            [ "left1",
              PlayerData.instance.hornet1Defeated
              && (PlayerData.instance.hasDash
                  || PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasAcidArmour),
              99.72462083156798,
              Dir.Sww,
              -0.5,
              6 ]
        | 134, "bot1" ->
            [ "right1", true, 21.319005605327842, Dir.Ne, 31.5, 13
              "top1", true, 86.28441342444185, Dir.N, 22, 85.5 ]
        | 134, "top1" ->
            [ "right1", true, 73.11976476986233, Dir.S, 31.5, 13
              "bot1", true, 86.28441342444185, Dir.S, 15, -0.5 ]
        | 134, "right1" ->
            [ "bot1", true, 21.319005605327842, Dir.Sw, 15, -0.5
              "top1", true, 73.11976476986233, Dir.N, 22, 85.5 ]
        | 135, "left1" -> [ "bot1", true, 104.96904305555995, Dir.E, 103, -0.5 ]
        | 135, "bot1" -> [ "left1", true, 104.96904305555995, Dir.W, -0.5, 17 ]
        | 136, "left1" ->
            [ "right1", true, 77.07788269017254, Dir.Nee, 70.5, 44
              "top1", true, 57.554322166106694, Dir.Ne, 36, 58.5 ]
        | 136, "top1" ->
            [ "right1", true, 37.42325480232846, Dir.See, 70.5, 44
              "left1", true, 57.554322166106694, Dir.Sw, -0.5, 14 ]
        | 136, "right1" ->
            [ "top1", true, 37.42325480232846, Dir.Nww, 36, 58.5
              "left1", true, 77.07788269017254, Dir.Sww, -0.5, 14 ]
        | 138, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasSuperDash
                  && PlayerData.instance.hasWalljump
                  && (PlayerData.instance.hasDash || PlayerData.instance.hasDoubleJump)
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash),
              251.01792764661252,
              Dir.W,
              -0.5,
              11 ]
        | 138, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              251.01792764661252,
              Dir.E,
              250.5,
              14 ]
        | 139, "top1" ->
            [ "right1", true, 137.07114940788964, Dir.E, 180.5, 13
              "left1", true, 45.96194077712559, Dir.Sww, -0.5, 14 ]
        | 139, "left1" ->
            [ "right1", true, 181.00276240985937, Dir.E, 180.5, 13
              "top1", true, 45.96194077712559, Dir.Nee, 44, 25.5 ]
        | 139, "right1" ->
            [ "left1", true, 181.00276240985937, Dir.W, -0.5, 14
              "top1", true, 137.07114940788964, Dir.W, 44, 25.5 ]
        | 141, "left1" -> [ "right1", true, 38.58756276314948, Dir.Nee, 32.5, 28 ]
        | 141, "right1" -> [ "left1", true, 38.58756276314948, Dir.Sww, -0.5, 8 ]
        | 144, "door1" -> [ "right1", true, 32.944878205875945, Dir.Se, 57.5, 8 ]
        | 144, "right1" ->
            [ "door1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash
                 && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              || PlayerData.instance.hasDash
                 && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              32.944878205875945,
              Dir.Nw,
              36.95,
              33.75 ]
        | 145, "door_stagExit" -> [ "right1", true, 31.432534100832534, Dir.E, 47.5, 7 ]
        | 146, "left1" -> [ "right1", true, 81.02468759581859, Dir.E, 80.5, 7 ]
        | 146, "right1" -> [ "left1", true, 81.02468759581859, Dir.W, -0.5, 9 ]
        | 147, "right1" ->
            [ "left1", true, 91.0, Dir.W, -0.5, 14
              "bot1", true, 27.613402542968153, Dir.Sww, 67, -0.5 ]
        | 147, "bot1" ->
            [ "left1", true, 69.03984356876832, Dir.Nww, -0.5, 14
              "right1", true, 27.613402542968153, Dir.Nee, 90.5, 14 ]
        | 147, "left1" ->
            [ "right1", true, 91.0, Dir.E, 90.5, 14
              "bot1", true, 69.03984356876832, Dir.See, 67, -0.5 ]
        | 148, "bot2" ->
            [ "bot1", true, 156.0, Dir.W, 7, -0.5
              "right1", true, 22.102036105300343, Dir.Ne, 180.5, 13 ]
        | 148, "right1" ->
            [ "bot1", true, 174.02442357324446, Dir.W, 7, -0.5
              "bot2", true, 22.102036105300343, Dir.Sw, 163, -0.5 ]
        | 148, "bot1" ->
            [ "bot2", true, 156.0, Dir.E, 163, -0.5
              "right1", true, 174.02442357324446, Dir.E, 180.5, 13 ]
        | 150, "top1" ->
            [ "left1", true, 18.560711193270585, Dir.Ssw, -0.5, 104
              "bot1", true, 122.98373876248843, Dir.S, 30, -0.5 ]
        | 150, "left1" ->
            [ "bot1", true, 108.8600018372221, Dir.Sse, 30, -0.5
              "top1", true, 18.560711193270585, Dir.Nne, 8, 120.5 ]
        | 151, "right1" -> [ "left1", true, 151.02979838429238, Dir.W, -0.5, 7 ]
        | 151, "left1" -> [ "right1", true, 151.02979838429238, Dir.E, 150.5, 10 ]
        | 153, "left1" -> [ "right1", true, 91.06728281880382, Dir.E, 90.5, 11 ]
        | 153, "right1" -> [ "left1", true, 91.06728281880382, Dir.W, -0.5, 14.5 ]
        | 154, "door_SlugShrine" ->
            [ "left1",
              PlayerData.instance.hasAcidArmour
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              78.8132476173898,
              Dir.W,
              -0.5,
              22
              "right1", true, 32.86362731044764, Dir.E, 110.5, 19.5 ]
        | 154, "right1" ->
            [ "left1",
              PlayerData.instance.hasAcidArmour
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              111.02814958378798,
              Dir.W,
              -0.5,
              22
              "door_SlugShrine", true, 32.86362731044764, Dir.W, 77.98, 14.76 ]
        | 154, "left1" ->
            [ "door_SlugShrine", PlayerData.instance.hasAcidArmour, 78.8132476173898, Dir.E, 77.98, 14.76
              "right1", PlayerData.instance.hasAcidArmour, 111.02814958378798, Dir.E, 110.5, 19.5 ]
        | 155, "left1" -> [ "left2", true, 41.0, Dir.S, -0.5, 18 ]
        | 155, "left2" ->
            [ "left1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump)
              && PlayerData.instance.defeatedDoubleBlockers
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              41.0,
              Dir.N,
              -0.5,
              59 ]
        | 156, "left1" -> [ "right1", true, 86.0, Dir.E, 85.5, 15 ]
        | 156, "right1" -> [ "left1", true, 86.0, Dir.W, -0.5, 15 ]
        | 158, "bot1" ->
            [ "right1", true, 117.52659273543158, Dir.N, 36.5, 116
              "top1", true, 134.18271125595876, Dir.N, 14, 133.5 ]
        | 158, "top1" ->
            [ "right1", true, 28.50438562747845, Dir.Se, 36.5, 116
              "bot1", true, 134.18271125595876, Dir.S, 21, -0.5 ]
        | 158, "right1" ->
            [ "bot1", true, 117.52659273543158, Dir.S, 21, -0.5
              "top1", true, 28.50438562747845, Dir.Nw, 14, 133.5 ]
        | 159, "top1" ->
            [ "left1", true, 77.61765263134411, Dir.Sww, -0.5, 4
              "bot1", true, 73.49829930005184, Dir.Sw, 7, -0.5 ]
        | 159, "bot1" ->
            [ "left1", true, 8.74642784226795, Dir.Nww, -0.5, 4
              "top1", true, 73.49829930005184, Dir.Ne, 68, 40.5 ]
        | 159, "left1" ->
            [ "top1", true, 77.61765263134411, Dir.Nee, 68, 40.5
              "bot1", true, 8.74642784226795, Dir.See, 7, -0.5 ]
        | 160, "left1" -> [ "door1", true, 103.66777946883978, Dir.E, 103.16, 3.73 ]
        | 160, "door1" -> [ "left1", true, 103.66777946883978, Dir.W, -0.5, 5 ]
        | 161, "left1" -> [ "right1", PlayerData.instance.hasLantern, 121.69634341261039, Dir.E, 120.5, 17 ]
        | 161, "right1" -> [ "left1", PlayerData.instance.hasLantern, 121.69634341261039, Dir.W, -0.5, 4 ]
        | 166, "door_stagExit" -> [ "right1", true, 31.39294347460907, Dir.E, 47.5, 7 ]
        | 168, "top1" -> [ "right1", true, 8.74642784226795, Dir.See, 36.5, 72 ]
        | 168, "right1" -> [ "top1", true, 8.74642784226795, Dir.Nww, 29, 76.5 ]
        | 169, "right1" -> [ "bot1", PlayerData.instance.notchShroomOgres, 69.03984356876832, Dir.Sww, 8, -0.5 ]
        | 169, "bot1" -> [ "right1", PlayerData.instance.notchShroomOgres, 69.03984356876832, Dir.Nee, 75.5, 14 ]
        | 170, "right1" -> [ "top1", true, 104.1849317319928, Dir.Nnw, 13, 160.5 ]
        | 170, "top1" -> [ "right1", true, 104.1849317319928, Dir.Sse, 36.5, 59 ]
        | 171, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasAcidArmour,
              71.56814934033156,
              Dir.W,
              -0.5,
              14 ]
        | 171, "left1" -> [ "right1", true, 71.56814934033156, Dir.E, 70.5, 5 ]
        | 172, "left2" ->
            [ "right1", true, 31.064449134018133, Dir.E, 30.5, 13
              "left1", true, 49.0, Dir.N, -0.5, 60 ]
        | 172, "left1" ->
            [ "right1", true, 56.302753041036986, Dir.Sse, 30.5, 13
              "left2", true, 49.0, Dir.S, -0.5, 11 ]
        | 172, "right1" ->
            [ "left1", true, 56.302753041036986, Dir.Nnw, -0.5, 60
              "left2", true, 31.064449134018133, Dir.W, -0.5, 11 ]
        | 173, "right1" -> [ "left1", true, 71.00704190430693, Dir.W, -0.5, 11 ]
        | 173, "left1" -> [ "right1", true, 71.00704190430693, Dir.E, 70.5, 12 ]
        | 174, "right2" ->
            [ "right1", true, 51.0, Dir.N, 70.5, 61
              "bot1", true, 65.3490627323759, Dir.W, 6, -0.5 ]
        | 174, "bot1" ->
            [ "right1", true, 89.12070466507768, Dir.Ne, 70.5, 61
              "right2", true, 65.3490627323759, Dir.E, 70.5, 10 ]
        | 174, "right1" ->
            [ "right2", true, 51.0, Dir.S, 70.5, 10
              "bot1", true, 89.12070466507768, Dir.Sw, 6, -0.5 ]
        | 176, "left1" -> [ "bot1", true, 91.89396062854185, Dir.E, 91, -0.5 ]
        | 176, "bot1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              91.89396062854185,
              Dir.W,
              -0.5,
              8 ]
        | 178, "top1" ->
            [ "bot3",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              48.877397639399746,
              Dir.Sse,
              60,
              -0.5 ]
        | 179, "top3" -> [ "left1", PlayerData.instance.defeatedMantisLords, 114.76606641337848, Dir.Ssw, 7, 10 ]
        | 179, "right1" -> [ "left1", PlayerData.instance.defeatedMantisLords, 95.15382283439799, Dir.Sw, 7, 10 ]
        | 182, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              46.09772228646444,
              Dir.W,
              -0.5,
              34 ]
        | 182, "bot1" ->
            [ "left1", PlayerData.instance.hasWalljump, 35.531676008879735, Dir.Nnw, -0.5, 34
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              53.033008588991066,
              Dir.Ne,
              45.5,
              37 ]
        | 182, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasDoubleJump,
              46.09772228646444,
              Dir.E,
              45.5,
              37 ]
        | 184, "left1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              50.798622028555066,
              Dir.E,
              50,
              25.5 ]
        | 184, "top1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash,
              50.798622028555066,
              Dir.W,
              -0.5,
              20 ]
        | 188, "top2" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              83.54190565219349,
              Dir.W,
              12,
              35.5
              "right1", PlayerData.instance.defeatedMantisLords, 76.17250159998686, Dir.See, 168.5, 6 ]
        | 188, "right1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              159.25608308632985,
              Dir.W,
              12,
              35.5 ]
        | 188, "top1" -> [ "right1", PlayerData.instance.defeatedMantisLords, 159.25608308632985, Dir.E, 168.5, 6 ]
        | 190, "left1" -> [ "left2", true, 15.0, Dir.S, -0.5, 5 ]
        | 190, "left2" -> [ "left1", true, 15.0, Dir.N, -0.5, 20 ]
        | 191, "bot1" -> [ "right1", true, 92.4040042422405, Dir.Nee, 94.5, 32 ]
        | 192, "top1" -> [ "bot1", true, 141.02216137898327, Dir.S, 17.5, -0.5 ]
        | 195, "left1" -> [ "right1", true, 82.13403679352427, Dir.Se, 60.5, 8 ]
        | 195, "right1" -> [ "left1", true, 82.13403679352427, Dir.Nw, -0.5, 63 ]
        | 199, "right1" -> [ "left1", PlayerData.instance.hasAcidArmour, 101.04454463255303, Dir.W, -0.5, 12 ]
        | 199, "left1" -> [ "right1", PlayerData.instance.hasAcidArmour, 101.04454463255303, Dir.E, 100.5, 9 ]
        | 200, "left2" -> [ "left1", true, 17.0, Dir.N, -0.5, 84 ]
        | 200, "left1" -> [ "left2", true, 17.0, Dir.S, -0.5, 67 ]
        | 201, "right1" ->
            [ "right2",
              PlayerData.instance.hasDoubleJump
              || (PlayerData.instance.hasDash
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash)
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              30.0,
              Dir.S,
              73.5,
              30
              "left1", true, 74.96665925596525, Dir.W, -0.5, 48 ]
        | 201, "left1" ->
            [ "right2",
              PlayerData.instance.hasDoubleJump
              || (PlayerData.instance.hasDash
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash)
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              76.15773105863909,
              Dir.See,
              73.5,
              30
              "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              74.96665925596525,
              Dir.E,
              73.5,
              60 ]
        | 202, "right1" ->
            [ "left1", true, 151.11915828246265, Dir.W, -0.5, 8
              "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              144.95689014324225,
              Dir.W,
              6,
              25.5 ]
        | 202, "top1" ->
            [ "left1", true, 18.66815470259447, Dir.Ssw, -0.5, 8
              "right1", true, 144.95689014324225, Dir.E, 150.5, 14 ]
        | 202, "left1" ->
            [ "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              18.66815470259447,
              Dir.Nne,
              6,
              25.5
              "right1", true, 151.11915828246265, Dir.E, 150.5, 14 ]
        | 205, "right1" ->
            [ "left3", true, 41.773197148410844, Dir.Sw, -0.5, 36
              "left1", true, 31.25699921617557, Dir.W, -0.5, 60 ]
        | 205, "left1" ->
            [ "left3", true, 24.0, Dir.S, -0.5, 36
              "right1", true, 31.25699921617557, Dir.E, 30.5, 64 ]
        | 205, "left3" ->
            [ "left1", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 24.0, Dir.N, -0.5, 60
              "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              41.773197148410844,
              Dir.Ne,
              30.5,
              64 ]
        | 206, "right1" ->
            [ "top1",
              PlayerData.instance.hasDash
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              69.16104394816492,
              Dir.Nww,
              9,
              26 ]
        | 206, "top1" -> [ "right1", PlayerData.instance.hasDash, 69.16104394816492, Dir.See, 75.5, 7 ]
        | 208, "right1" ->
            [ "left1",
              PlayerData.instance.hasShadowDash
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.killedTraitorLord,
              103.36827366266692,
              Dir.Nww,
              -0.5,
              28 ]
        | 208, "left1" ->
            [ "right1",
              PlayerData.instance.hasShadowDash && PlayerData.instance.killedTraitorLord,
              103.36827366266692,
              Dir.See,
              100.5,
              6 ]
        | 210, "top1" ->
            [ "left1", true, 35.16390194503448, Dir.Sww, -0.5, 13
              "right1", true, 72.63952092353033, Dir.See, 100.5, 13 ]
        | 210, "right1" ->
            [ "left1", true, 101.0, Dir.W, -0.5, 13
              "top1", true, 72.63952092353033, Dir.Nww, 30, 30.5 ]
        | 210, "left1" ->
            [ "top1", true, 35.16390194503448, Dir.Nee, 30, 30.5
              "right1", true, 101.0, Dir.E, 100.5, 13 ]
        | 211, "left1" -> [ "right1", PlayerData.instance.hasShadowDash, 101.07917688624102, Dir.E, 100.5, 16 ]
        | 211, "right1" -> [ "left1", PlayerData.instance.hasShadowDash, 101.07917688624102, Dir.W, -0.5, 20 ]
        | 212, "right1" -> [ "left1", true, 101.0, Dir.W, -0.5, 16 ]
        | 212, "left1" -> [ "right1", true, 101.0, Dir.E, 100.5, 16 ]
        | 214, "left1" -> [ "right1", true, 76.16429609731846, Dir.E, 75.5, 9 ]
        | 214, "right1" -> [ "left1", true, 76.16429609731846, Dir.W, -0.5, 4 ]
        | 219, "left1" -> [ "right1", true, 117.39250401963491, Dir.See, 109.5, 7 ]
        | 220, "door_stagExit" ->
            [ "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              139.1916912031749,
              Dir.W,
              19,
              35.5
              "right1", true, 31.230064040920556, Dir.E, 187.5, 14 ]
        | 220, "right1" ->
            [ "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              169.8661237563276,
              Dir.W,
              19,
              35.5 ]
        | 220, "top1" ->
            [ "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              169.8661237563276,
              Dir.E,
              187.5,
              14 ]
        | 224, "right2" -> [ "bot1", true, 43.982951242498494, Dir.Sw, 62, -0.5 ]
        | 224, "door1" -> [ "right1", true, 57.494992625445214, Dir.E, 95.5, 95 ]
        | 224, "bot1" ->
            [ "right2",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              43.982951242498494,
              Dir.Ne,
              95.5,
              28 ]
        | 227, "left1" -> [ "bot1", true, 68.90936075744717, Dir.E, 68, -0.5 ]
        | 227, "bot1" -> [ "left1", true, 68.90936075744717, Dir.W, -0.5, 7 ]
        | 230, "right1" ->
            [ "right2", true, 52.0, Dir.S, 140.5, 92
              "right4", true, 112.0, Dir.S, 140.5, 32 ]
        | 230, "right2" -> [ "right4", true, 60.0, Dir.S, 140.5, 32 ]
        | 231, "bot1" -> [ "bot2", true, 33.53356527421443, Dir.E, 221.5, -2 ]
        | 231, "door1" -> [ "left1", true, 124.55380845241145, Dir.W, -0.5, 25 ]
        | 231, "left1" -> [ "door1", true, 124.55380845241145, Dir.E, 122.54, 5.64 ]
        | 233, "door_stagExit" -> [ "right1", true, 172.4869299396334, Dir.Nee, 180.5, 58 ]
        | 234, "left1" -> [ "right1", PlayerData.instance.hasLantern, 86.14522621712709, Dir.E, 85.5, 49 ]
        | 234, "right1" ->
            [ "left1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              86.14522621712709,
              Dir.W,
              -0.5,
              44 ]
        | 239, "left1" -> [ "right1", true, 111.0, Dir.E, 110.5, 6 ]
        | 239, "right1" -> [ "left1", true, 111.0, Dir.W, -0.5, 6 ]
        | 241, "right1" ->
            [ "left1", true, 111.0, Dir.W, -0.5, 8
              "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              104.94045930907679,
              Dir.Nww,
              8,
              30.5 ]
        | 241, "top1" ->
            [ "left1", true, 24.052026941611388, Dir.Ssw, -0.5, 8
              "right1", true, 104.94045930907679, Dir.See, 110.5, 8 ]
        | 241, "left1" ->
            [ "right1", true, 111.0, Dir.E, 110.5, 8
              "top1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              24.052026941611388,
              Dir.Nne,
              8,
              30.5 ]
        | 244, "door_stagExit" -> [ "left1", true, 32.30458171838788, Dir.W, -0.5, 7 ]
        | 246, "door_Mansion" -> [ "bot1", true, 42.573467089256425, Dir.W, 20.5, -0.5 ]
        | 246, "bot1" -> [ "door_Mansion", true, 42.573467089256425, Dir.E, 62.95, 2.74 ]
        | 250, "right1" -> [ "top1", PlayerData.instance.hasWalljump, 51.79285665031424, Dir.Nw, 12, 80.5 ]
        | 250, "top1" -> [ "right1", true, 51.79285665031424, Dir.Se, 45.5, 41 ]
        | 251, "top1" -> [ "left1", true, 29.129023327259016, Dir.Sw, -0.5, 67 ]
        | 252, "left1" ->
            [ "right1", true, 50.91168824543142, Dir.Se, 35.5, 33
              "top1", true, 16.446884203398525, Dir.Nee, 15, 74.5 ]
        | 252, "top1" ->
            [ "right1", true, 46.28714724413247, Dir.Sse, 35.5, 33
              "left1", true, 16.446884203398525, Dir.Sww, -0.5, 69 ]
        | 252, "right1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              46.28714724413247,
              Dir.Nnw,
              15,
              74.5
              "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              50.91168824543142,
              Dir.Nw,
              -0.5,
              69 ]
        | 253, "right1" ->
            [ "left1",
              PlayerData.instance.hasSuperDash
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              151.08275877809487,
              Dir.W,
              -0.5,
              16 ]
        | 253, "left1" -> [ "right1", true, 151.08275877809487, Dir.E, 150.5, 11 ]
        | 254, "right1" -> [ "left1", PlayerData.instance.hasLantern, 121.00413216084813, Dir.W, -0.5, 7.0 ]
        | 254, "left1" -> [ "right1", PlayerData.instance.hasLantern, 121.00413216084813, Dir.E, 120.5, 6.0 ]
        | 255, "bot1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              && PlayerData.instance.hasSuperDash
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDash && PlayerData.instance.hasDoubleJump),
              155.92466129512675,
              Dir.E,
              187.5,
              11
              "left1",
              PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              34.4746283518764,
              Dir.Nww,
              -0.5,
              11 ]
        | 255, "left1" ->
            [ "bot1",
              PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash,
              34.4746283518764,
              Dir.See,
              32,
              -0.5 ]
        | 255, "right1" ->
            [ "bot1",
              PlayerData.instance.hasSuperDash
              && PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash)
              && (PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash),
              155.92466129512675,
              Dir.W,
              32,
              -0.5 ]
        | 257, "bot1" ->
            [ "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              71.43178564196754,
              Dir.Nee,
              100.5,
              16 ]
        | 257, "top1" ->
            [ "right1", true, 36.76955262170047, Dir.Sse, 100.5, 16
              "bot1", true, 75.03665770808293, Dir.Sw, 31, -0.5 ]
        | 257, "right1" -> [ "bot1", true, 71.43178564196754, Dir.Sww, 31, -0.5 ]
        | 259, "left1" -> [ "right1", true, 76.0, Dir.E, 75.5, 12 ]
        | 259, "right1" -> [ "left1", true, 76.0, Dir.W, -0.5, 12 ]
        | 262, "left1" -> [ "right1", true, 61.0, Dir.E, 60.5, 7 ]
        | 262, "right1" -> [ "left1", true, 61.0, Dir.W, -0.5, 7 ]
        | 263, "left2" ->
            [ "bot1", true, 72.2530276182251, Dir.Sse, 39, 59.5
              "left3", true, 56.0, Dir.S, -0.5, 64 ]
        | 263, "right2" ->
            [ "bot1", true, 36.776351096866584, Dir.W, 39, 59.5
              "left3", true, 76.0, Dir.W, -0.5, 64 ]
        | 263, "left3" -> [ "bot1", true, 39.75550276376844, Dir.E, 39, 59.5 ]
        | 263, "bot1" -> [ "left3", true, 39.75550276376844, Dir.W, -0.5, 64 ]
        | 264, "right1" ->
            [ "left1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasWalljump,
              181.62048342629197,
              Dir.W,
              -0.5,
              13
              "right2", true, 22.0, Dir.S, 180.5, 6 ]
        | 264, "top1" ->
            [ "left1", true, 42.9563732174866, Dir.Sw, -0.5, 13
              "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump,
              148.52693358445129,
              Dir.E,
              180.5,
              28
              "right2",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump,
              151.96792424719106,
              Dir.See,
              180.5,
              6 ]
        | 264, "right2" ->
            [ "left1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              181.13530854032848,
              Dir.W,
              -0.5,
              13
              "right1",
              PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash,
              22.0,
              Dir.N,
              180.5,
              28 ]
        | 264, "left1" ->
            [ "right1",
              PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash && PlayerData.instance.hasWalljump
              || PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump,
              181.62048342629197,
              Dir.E,
              180.5,
              28
              "right2",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              181.13530854032848,
              Dir.E,
              180.5,
              6 ]
        | 266, "left1" ->
            [ "top1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump)
              && PlayerData.instance.hasDoubleJump,
              109.69275272323145,
              Dir.N,
              6,
              130.5 ]
        | 266, "top1" -> [ "left1", true, 109.69275272323145, Dir.S, -0.5, 21 ]
        | 267, "left1" ->
            [ "door1",
              PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash,
              81.71268934504603,
              Dir.E,
              81.2,
              47.56
              "bot1", true, 53.57704732439069, Dir.Sse, 20, -0.5 ]
        | 267, "door1" ->
            [ "left1",
              PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash,
              81.71268934504603,
              Dir.W,
              -0.5,
              49
              "bot1", true, 77.81518874872695, Dir.Sw, 20, -0.5 ]
        | 268, "right2" ->
            [ "right1", true, 44.0, Dir.N, 45.5, 57
              "left1", true, 46.87216658103186, Dir.W, -0.5, 4 ]
        | 268, "left1" ->
            [ "right1", true, 70.178344238091, Dir.Ne, 45.5, 57
              "right2", true, 46.87216658103186, Dir.E, 45.5, 13 ]
        | 268, "right1" ->
            [ "right2", true, 44.0, Dir.S, 45.5, 13
              "left1", true, 70.178344238091, Dir.Sw, -0.5, 4 ]
        | 269, "left1" -> [ "right1", PlayerData.instance.hasSuperDash, 181.01104938649465, Dir.E, 180.5, 11 ]
        | 269, "right1" -> [ "left1", PlayerData.instance.hasSuperDash, 181.01104938649465, Dir.W, -0.5, 9 ]
        | 272, "left1" -> [ "right1", PlayerData.instance.hasLantern, 101.00495037373169, Dir.E, 100.5, 8 ]
        | 272, "right1" -> [ "left1", PlayerData.instance.hasLantern, 101.00495037373169, Dir.W, -0.5, 9 ]
        | 273, "bot1" ->
            [ "left1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump)
              && (PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasDash)
              && (PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasDash),
              174.27134015666488,
              Dir.Nww,
              -3.5,
              40
              "bot2",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump)
              && (PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasDash),
              150.50747489742827,
              Dir.W,
              15.5,
              -2 ]
        | 276, "bot1" -> [ "top1", PlayerData.instance.hasWalljump, 66.27216610312357, Dir.N, 38, 65.5 ]
        | 276, "top1" -> [ "bot1", PlayerData.instance.hasDash, 66.27216610312357, Dir.S, 32, -0.5 ]
        | 278, "top1" -> [ "right2", true, 69.38659812961002, Dir.Se, 60.5, 37 ]
        | 278, "top2" -> [ "right2", true, 56.68553607402862, Dir.Sse, 60.5, 37 ]
        | 278, "right1" ->
            [ "top1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump,
              54.39209501388966,
              Dir.Nww,
              9,
              83.5 ]
        | 279, "left1" -> [ "right1", true, 38.8329756778952, Dir.Se, 31.5, 45 ]
        | 280, "top1" -> [ "left1", true, 81.50153372789987, Dir.Sw, -0.5, 48 ]
        | 280, "left1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              81.50153372789987,
              Dir.Ne,
              48,
              113.5 ]
        | 281, "door_stagExit" -> [ "left1", true, 32.354934708634474, Dir.W, -0.5, 7 ]
        | 282, "door1" ->
            [ "door2", true, 26.461725189412732, Dir.Ssw, 19.82, 94.49
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash,
              47.74030791689555,
              Dir.Nee,
              75.5,
              139 ]
        | 282, "right1" -> [ "door2", true, 71.28395682059183, Dir.Sw, 19.82, 94.49 ]
        | 283, "left1" -> [ "right1", true, 83.18653737234169, Dir.See, 80.5, 4 ]
        | 284, "bot1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              73.98986417070923,
              Dir.W,
              -0.5,
              8 ]
        | 288, "right1" -> [ "left1", true, 121.0, Dir.W, -0.5, 6 ]
        | 288, "top1" ->
            [ "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              154.8628425413921,
              Dir.S,
              -0.5,
              6
              "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              183.47343131908772,
              Dir.Sse,
              120.5,
              6 ]
        | 288, "left1" -> [ "right1", true, 121.0, Dir.E, 120.5, 6 ]
        | 289, "right1" -> [ "right2", PlayerData.instance.hasWalljump, 48.0, Dir.S, 125.5, 5 ]
        | 289, "right2" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash,
              48.0,
              Dir.N,
              125.5,
              53 ]
        | 291, "top1" ->
            [ "bot1", true, 74.81310045707235, Dir.Se, 73, -0.5
              "top2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 59.0, Dir.E, 73, 45.5 ]
        | 291, "top2" -> [ "bot1", true, 46.0, Dir.S, 73, -0.5 ]
        | 291, "bot1" ->
            [ "top2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 46.0, Dir.N, 73, 45.5 ]
        | 297, "top1" ->
            [ "door1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasDash
                  || PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash),
              126.97146647967803,
              Dir.See,
              195.93,
              38.72
              "right1", PlayerData.instance.hasLantern, 151.03807466993214, Dir.See, 210.5, 8
              "left1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasDash
                  || PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump),
              74.7295122424869,
              Dir.W,
              -0.5,
              57 ]
        | 297, "right1" ->
            [ "door1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump)
              && PlayerData.instance.hasLantern
              && (PlayerData.instance.hasDash
                  || PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash),
              34.00004852937713,
              Dir.Nnw,
              195.93,
              38.72
              "top1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump),
              151.03807466993214,
              Dir.Nww,
              73,
              70.5
              "left1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump),
              216.61486560252507,
              Dir.Nww,
              -0.5,
              57 ]
        | 297, "left1" ->
            [ "door1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasDash
                  || PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash),
              197.27874518051863,
              Dir.E,
              195.93,
              38.72
              "right1", PlayerData.instance.hasLantern, 216.61486560252507, Dir.See, 210.5, 8
              "top1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump
                     && (PlayerData.instance.hasDash || PlayerData.instance.hasWalljump)),
              74.7295122424869,
              Dir.E,
              73,
              70.5 ]
        | 297, "door1" ->
            [ "right1", PlayerData.instance.hasLantern, 34.00004852937713, Dir.Sse, 210.5, 8
              "top1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasWalljump
                     && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
                     && PlayerData.instance.hasDash),
              126.97146647967803,
              Dir.Nww,
              73,
              70.5
              "left1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasWalljump
                     && (PlayerData.instance.hasDoubleJump
                         || PlayerData.instance.hasDash
                         || PlayerData.instance.hasSuperDash)),
              197.27874518051863,
              Dir.W,
              -0.5,
              57 ]
        | 299, "left1" ->
            [ "right1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump),
              121.03718436910205,
              Dir.E,
              120.5,
              88 ]
        | 299, "left2" ->
            [ "right1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump),
              146.16771189288008,
              Dir.Ne,
              120.5,
              88
              "left1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump),
              85.0,
              Dir.N,
              -0.5,
              91 ]
        | 299, "right1" ->
            [ "left1",
              PlayerData.instance.hasLantern
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump),
              121.03718436910205,
              Dir.W,
              -0.5,
              91 ]
        | 301, "right1" ->
            [ "left1", true, 41.0, Dir.W, -0.5, 116
              "bot1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              121.22087279012636,
              Dir.Ssw,
              7,
              -0.5 ]
        | 301, "bot1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              116.74116668939025,
              Dir.N,
              -0.5,
              116
              "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              121.22087279012636,
              Dir.Nne,
              40.5,
              116 ]
        | 301, "left1" ->
            [ "bot1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              116.74116668939025,
              Dir.S,
              7,
              -0.5
              "right1", true, 41.0, Dir.E, 40.5, 116 ]
        | 306, "right1" ->
            [ "top1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump,
              55.2675311552814,
              Dir.Nnw,
              23,
              85.5
              "bot1", true, 39.85599076675927, Dir.Ssw, 36, -0.5 ]
        | 306, "bot1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              86.97700845625813,
              Dir.N,
              23,
              85.5 ]
        | 306, "top1" -> [ "bot1", true, 86.97700845625813, Dir.S, 36, -0.5 ]
        | 308, "top2" -> [ "right1", true, 48.654393429576324, Dir.See, 80.5, 132 ]
        | 309, "left2" -> [ "right2", PlayerData.instance.hasDoubleJump, 56.61271941887264, Dir.Ne, 45.5, 127 ]
        | 309, "right2" -> [ "left2", true, 56.61271941887264, Dir.Sw, -0.5, 94 ]
        | 310, "left1" ->
            [ "right1", PlayerData.instance.hasWalljump, 352.00568177232594, Dir.E, 351.5, 16
              "top1", PlayerData.instance.hasWalljump, 137.6971314152913, Dir.Nee, 134, 43.5
              "door1", PlayerData.instance.hasWalljump, 329.7207644356055, Dir.E, 329.22, 14.71 ]
        | 310, "bot1" ->
            [ "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              132.45754036671525,
              Dir.E,
              351.5,
              16
              "top1", PlayerData.instance.hasWalljump, 93.7683315410912, Dir.Nww, 134, 43.5
              "door1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              110.14881978487108,
              Dir.E,
              329.22,
              14.71
              "left1", PlayerData.instance.hasWalljump, 220.18401395196702, Dir.W, -0.5, 14 ]
        | 310, "door1" ->
            [ "right1", true, 22.31731390647178, Dir.E, 351.5, 16
              "top1", PlayerData.instance.hasWalljump, 197.33147873565437, Dir.W, 134, 43.5
              "left1", PlayerData.instance.hasWalljump, 329.7207644356055, Dir.W, -0.5, 14 ]
        | 310, "top1" ->
            [ "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              219.23161268393753,
              Dir.E,
              351.5,
              16
              "door1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              197.33147873565437,
              Dir.E,
              329.22,
              14.71
              "left1", true, 137.6971314152913, Dir.Sww, -0.5, 14 ]
        | 310, "right1" ->
            [ "top1", PlayerData.instance.hasWalljump, 219.23161268393753, Dir.W, 134, 43.5
              "door1", true, 22.31731390647178, Dir.W, 329.22, 14.71
              "left1", PlayerData.instance.hasWalljump, 352.00568177232594, Dir.W, -0.5, 14 ]
        | 312, "right1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              118.49261580368626,
              Dir.Nww,
              6,
              40.5 ]
        | 312, "top1" -> [ "right1", true, 118.49261580368626, Dir.See, 120.5, 10 ]
        | 313, "bot1" ->
            [ "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              87.70689824637512,
              Dir.E,
              171.5,
              14
              "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              86.89361311396829,
              Dir.W,
              -0.5,
              15 ]
        | 313, "right1" ->
            [ "left1", true, 172.00290695217916, Dir.W, -0.5, 15
              "bot1", true, 87.70689824637512, Dir.W, 85, -0.5 ]
        | 315, "top1" ->
            [ "bot1", true, 100.72239075796404, Dir.Ssw, 37, 38.5
              "left1", true, 89.74686624055461, Dir.Sww, -0.5, 87
              "right1", true, 34.15406271587613, Dir.See, 110.5, 120 ]
        | 315, "left1" ->
            [ "bot1", true, 61.30660649554826, Dir.Se, 37, 38.5
              "top1",
              PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              89.74686624055461,
              Dir.Nee,
              78,
              130.5
              "right1", PlayerData.instance.hasDoubleJump, 115.80155439371269, Dir.Nee, 110.5, 120 ]
        | 315, "right1" ->
            [ "bot1", true, 109.74743732771167, Dir.Sw, 37, 38.5
              "top1", PlayerData.instance.hasWalljump, 34.15406271587613, Dir.Nww, 78, 130.5
              "left1", true, 115.80155439371269, Dir.Sww, -0.5, 87 ]
        | 315, "bot1" ->
            [ "top1",
              PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              100.72239075796404,
              Dir.Nne,
              78,
              130.5
              "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              61.30660649554826,
              Dir.Nw,
              -0.5,
              87
              "right1", PlayerData.instance.hasDoubleJump, 109.74743732771167, Dir.Ne, 110.5, 120 ]
        | 316, "left1" -> [ "right1", true, 121.0, Dir.E, 120.5, 12 ]
        | 316, "right1" -> [ "left1", true, 121.0, Dir.W, -0.5, 12 ]
        | 318, "left1" -> [ "door1", PlayerData.instance.hasDash, 87.68741072696811, Dir.E, 136.68, 6.86 ]
        | 321, "bot1" -> [ "left1", true, 42.573465914816005, Dir.W, -0.5, 4 ]
        | 324, "left1" -> [ "left2", PlayerData.instance.hornetOutskirtsDefeated, 77.28195132112025, Dir.Sse, 16, 7.5 ]
        | 327, "right1" ->
            [ "left1",
              PlayerData.instance.dungDefenderWallBroken
              || PlayerData.instance.dungDefenderWallBroken
                 && (PlayerData.instance.hasWalljump
                     && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
                     || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
                     || PlayerData.instance.hasDoubleJump),
              41.773197148410844,
              Dir.Sw,
              -0.5,
              133 ]
        | 327, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDoubleJump || PlayerData.instance.hasDash)
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash,
              41.773197148410844,
              Dir.Ne,
              30.5,
              161 ]
        | 327, "right2" -> [ "left2", PlayerData.instance.hasSuperDash, 31.0, Dir.W, -0.5, 51 ]
        | 327, "left2" -> [ "right2", PlayerData.instance.hasSuperDash, 31.0, Dir.E, 30.5, 51 ]
        | 328, "right1" -> [ "bot1", true, 163.20079656668347, Dir.W, 9, -0.5 ]
        | 328, "bot1" ->
            [ "right1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasDash
                  || PlayerData.instance.hasSuperDash),
              163.20079656668347,
              Dir.E,
              170.5,
              23 ]
        | 329, "door_tram_arrive" ->
            [ "top1", true, 35.64172017584163, Dir.Nww, 47, 22.5
              "bot1", true, 67.10215784230039, Dir.W, 14, 0
              "bot2", true, 18.10238059187233, Dir.W, 62.5, 7 ]
        | 330, "door_tram_arrive" -> [ "left1", true, 32.84000152253346, Dir.W, -0.5, 10 ]
        | 331, "door_tram_arrive" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              96.81427838908887,
              Dir.E,
              140,
              25.5
              "right1", true, 106.08003817872616, Dir.E, 150.5, 10 ]
        | 334, "top1" ->
            [ "left1",
              PlayerData.instance.hasKingsBrand
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              158.24190342636808,
              Dir.Sw,
              0.5,
              140 ]
        | 337, "left1" ->
            [ "left2",
              PlayerData.instance.hasShadowDash
              && (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump),
              31.0,
              Dir.N,
              -0.5,
              42 ]
        | 337, "left2" -> [ "left1", PlayerData.instance.hasShadowDash, 31.0, Dir.S, -0.5, 11 ]
        | 340, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash,
              101.00495037373169,
              Dir.E,
              100.5,
              9 ]
        | 340, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasSuperDash,
              101.00495037373169,
              Dir.W,
              -0.5,
              8 ]
        | 342, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash,
              201.08953229842672,
              Dir.E,
              200.5,
              12 ]
        | 342, "right1" ->
            [ "left1",
              PlayerData.instance.hasDoubleJump || PlayerData.instance.hasSuperDash,
              201.08953229842672,
              Dir.W,
              -0.5,
              18 ]
        | 343, "bot1" ->
            [ "right1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              83.117386869415,
              Dir.Nee,
              160.5,
              32
              "left1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.killedInfectedKnight,
              89.33784192602819,
              Dir.Nww,
              -0.5,
              28.5
              "bot2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 34.5, Dir.E, 118.5, -0.5 ]
        | 343, "bot2" -> [ "right1", PlayerData.instance.hasDoubleJump, 53.10602602341847, Dir.Ne, 160.5, 32 ]
        | 343, "right1" -> [ "bot2", true, 53.10602602341847, Dir.Sw, 118.5, -0.5 ]
        | 344, "top2" -> [ "top1", PlayerData.instance.hasWalljump, 35.5, Dir.W, 83, 70.5 ]
        | 344, "top1" -> [ "top2", PlayerData.instance.hasWalljump, 35.5, Dir.E, 118.5, 70.5 ]
        | 346, "door_stagExit" -> [ "left1", true, 36.12560864539171, Dir.W, -0.5, 7 ]
        | 349, "top1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDash,
              75.73803535872844,
              Dir.E,
              152.5,
              41 ]
        | 350, "top1" -> [ "bot2", true, 161.68178623456632, Dir.See, 220, -0.5 ]
        | 350, "top2" -> [ "bot2", true, 67.20863039818622, Dir.Se, 220, -0.5 ]
        | 350, "bot1" ->
            [ "bot2", PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump, 211.0, Dir.E, 220, -0.5 ]
        | 353, "left1" ->
            [ "right2",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasAcidArmour,
              131.7459676802292,
              Dir.E,
              130.5,
              9
              "right1", PlayerData.instance.hasDoubleJump, 131.4610208388783, Dir.E, 130.5, 34 ]
        | 353, "right1" ->
            [ "right2",
              (PlayerData.instance.hasDoubleJump
               || PlayerData.instance.hasDash
               || PlayerData.instance.hasAcidArmour
               || PlayerData.instance.hasWalljump
                  && PlayerData.instance.hasDash
                  && PlayerData.instance.hasSuperDash)
              && (PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasAcidArmour),
              25.0,
              Dir.S,
              130.5,
              9
              "left1",
              PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasDash
              || PlayerData.instance.hasAcidArmour
              || PlayerData.instance.hasWalljump
                 && PlayerData.instance.hasDash
                 && PlayerData.instance.hasSuperDash,
              131.4610208388783,
              Dir.W,
              -0.5,
              23 ]
        | 353, "right2" ->
            [ "left1",
              PlayerData.instance.hasDoubleJump
              && PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasAcidArmour
                  || PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump),
              131.7459676802292,
              Dir.W,
              -0.5,
              23
              "right1",
              PlayerData.instance.hasDoubleJump
              && PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasAcidArmour
                  || PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump),
              25.0,
              Dir.N,
              130.5,
              34 ]
        | 354, "bot1" ->
            [ "right1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.defeatedDungDefender,
              112.98893751159889,
              Dir.E,
              120.5,
              10 ]
        | 354, "bot2" -> [ "right1", true, 17.92344832893492, Dir.Nee, 120.5, 10 ]
        | 356, "top1" ->
            [ "right1",
              PlayerData.instance.hasAcidArmour
              && (PlayerData.instance.hasDoubleJump || PlayerData.instance.hasWalljump)
              || PlayerData.instance.hasSuperDash,
              57.554322166106694,
              Dir.Se,
              140.5,
              18 ]
        | 356, "right1" ->
            [ "top1",
              (PlayerData.instance.hasAcidArmour || PlayerData.instance.hasSuperDash)
              && (PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
                  || PlayerData.instance.hasDoubleJump),
              57.554322166106694,
              Dir.Nw,
              93,
              50.5 ]
        | 357, "door1" ->
            [ "right1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.waterwaysAcidDrained,
              47.00206910339161,
              Dir.Se,
              110.5,
              50 ]
        | 357, "top1" ->
            [ "right1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.waterwaysAcidDrained,
              111.58180855318666,
              Dir.See,
              110.5,
              50
              "door1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              71.4591106857621,
              Dir.E,
              81.11,
              86.68 ]
        | 357, "right1" -> [ "door1", PlayerData.instance.hasWalljump, 47.00206910339161, Dir.Nw, 81.11, 86.68 ]
        | 358, "left2" ->
            [ "left1", true, 48.00260409602796, Dir.S, -0.5, 7
              "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              25.59785147234041,
              Dir.Nee,
              25,
              60.5 ]
        | 358, "top1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasSuperDash,
              59.26634795564849,
              Dir.Ssw,
              -0.5,
              7 ]
        | 358, "left1" ->
            [ "top1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              59.26634795564849,
              Dir.Nne,
              25,
              60.5 ]
        | 359, "left1" -> [ "right1", true, 51.178608812667036, Dir.Nee, 50.5, 20 ]
        | 359, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              51.178608812667036,
              Dir.Sww,
              1,
              7 ]
        | 363, "bot2" ->
            [ "bot1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump)
              && (PlayerData.instance.hasAcidArmour || PlayerData.instance.hasSuperDash),
              241.0,
              Dir.W,
              8,
              -0.5 ]
        | 363, "bot1" ->
            [ "bot2",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasAcidArmour || PlayerData.instance.hasSuperDash),
              241.0,
              Dir.E,
              249,
              -0.5 ]
        | 365, "top1" ->
            [ "right1",
              PlayerData.instance.hasDash
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.whitePalaceOrb_1,
              148.99077152629286,
              Dir.Se,
              200.5,
              24 ]
        | 368, "right2" ->
            [ "top1",
              PlayerData.instance.hasDash
              && PlayerData.instance.hasDoubleJump
              && PlayerData.instance.hasWalljump,
              189.50593658247226,
              Dir.Nww,
              19,
              78.5 ]
        | 368, "top1" ->
            [ "right2",
              PlayerData.instance.hasDoubleJump
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              189.50593658247226,
              Dir.See,
              200.5,
              24 ]
        | 369, "right1" ->
            [ "left1",
              (PlayerData.instance.hasDash || PlayerData.instance.hasDoubleJump)
              && (PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump),
              106.0,
              Dir.W,
              50.5,
              115 ]
        | 369, "left1" ->
            [ "right1", PlayerData.instance.hasDash || PlayerData.instance.hasDoubleJump, 106.0, Dir.E, 156.5, 115 ]
        | 369, "left2" -> [ "right2", true, 108.0, Dir.E, 156.5, 19 ]
        | 369, "right2" -> [ "left2", true, 108.0, Dir.W, 48.5, 19 ]
        | 370, "top1" ->
            [ "left1", true, 70.26023057178222, Dir.Ssw, -0.5, 8
              "bot1", true, 76.0, Dir.S, 19, -0.5 ]
        | 370, "bot1" ->
            [ "left1", true, 21.27204738618265, Dir.Nww, -0.5, 8
              "top1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDoubleJump || PlayerData.instance.hasWalljump),
              76.0,
              Dir.N,
              19,
              75.5 ]
        | 370, "left1" ->
            [ "bot1", true, 21.27204738618265, Dir.See, 19, -0.5
              "top1",
              PlayerData.instance.hasWalljump
              && (PlayerData.instance.hasDoubleJump || PlayerData.instance.hasWalljump),
              70.26023057178222,
              Dir.Nne,
              19,
              75.5 ]
        | 371, "bot1" ->
            [ "top1",
              (PlayerData.instance.hasWalljump
               || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash)
              && PlayerData.instance.hasDoubleJump,
              142.41137595009747,
              Dir.N,
              31,
              140.5 ]
        | 371, "top1" -> [ "bot1", true, 142.41137595009747, Dir.S, 51, -0.5 ]
        | 372, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              154.9653187006693,
              Dir.E,
              154,
              21 ]
        | 372, "right1" -> [ "left1", true, 154.9653187006693, Dir.W, -0.5, 9 ]
        | 375, "bot1" ->
            [ "right1",
              (PlayerData.instance.hasDash
               || PlayerData.instance.hasSuperDash && PlayerData.instance.hasWalljump
               || PlayerData.instance.hasDoubleJump)
              && PlayerData.instance.hasWalljump
              && PlayerData.instance.hasSuperDash,
              40.697051490249265,
              Dir.E,
              74.5,
              136 ]
        | 376, "left1" ->
            [ "right1",
              PlayerData.instance.hasSuperDash
              && (PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasDash && PlayerData.instance.hasWalljump),
              193.32873557751316,
              Dir.See,
              175.5,
              151 ]
        | 377, "right1" ->
            [ "bot1",
              (PlayerData.instance.hasDash
               || PlayerData.instance.hasSuperDash
               || PlayerData.instance.hasDoubleJump)
              && (PlayerData.instance.hasWalljump
                  || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump)
              && PlayerData.instance.hasDash
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash),
              183.92525655820083,
              Dir.W,
              17,
              68.5 ]
        | 377, "bot1" ->
            [ "right1",
              (PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump)
              && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              && (PlayerData.instance.hasDash
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash)
              && (PlayerData.instance.hasDoubleJump
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasSuperDash
                  || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash),
              183.92525655820083,
              Dir.E,
              200.5,
              81 ]
        | 378, "right1" ->
            [ "left1", true, 119.4026800369238, Dir.Sw, -0.5, 19
              "right2", true, 96.0, Dir.S, 70.5, 19 ]
        | 378, "right2" ->
            [ "left1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump && PlayerData.instance.hasWalljump,
              71.0,
              Dir.W,
              -0.5,
              19 ]
        | 378, "left1" -> [ "right2", PlayerData.instance.hasWalljump, 71.0, Dir.E, 70.5, 19 ]
        | 379, "left2" ->
            [ "left1",
              PlayerData.instance.hasWalljump || PlayerData.instance.hasDoubleJump,
              96.00520819205592,
              Dir.N,
              135.5,
              115 ]
        | 379, "left1" -> [ "left2", true, 96.00520819205592, Dir.S, 134.5, 19 ]
        | 380, "bot1" ->
            [ "right1",
              PlayerData.instance.hasDash
              && PlayerData.instance.hasWalljump
              && PlayerData.instance.hasDoubleJump,
              53.2024435529046,
              Dir.Ne,
              120.5,
              34 ]
        | 381, "right1" ->
            [ "top1",
              PlayerData.instance.hasDash
              && PlayerData.instance.hasWalljump
              && PlayerData.instance.hasDoubleJump
              && PlayerData.instance.hasSuperDash,
              224.7187130614627,
              Dir.W,
              78,
              44.5 ]
        | 381, "top1" ->
            [ "right1",
              PlayerData.instance.hasDash
              && PlayerData.instance.hasWalljump
              && PlayerData.instance.hasDoubleJump
              && PlayerData.instance.hasSuperDash,
              224.7187130614627,
              Dir.E,
              300.5,
              13 ]
        | 382, "left1" ->
            [ "top1",
              PlayerData.instance.hasDash
              && PlayerData.instance.hasWalljump
              && PlayerData.instance.hasSuperDash
              && PlayerData.instance.hasDoubleJump,
              151.16629915427578,
              Dir.Nnw,
              18,
              168.5 ]
        | 382, "top1" ->
            [ "left1",
              PlayerData.instance.hasDash
              && PlayerData.instance.hasWalljump
              && PlayerData.instance.hasDoubleJump,
              151.16629915427578,
              Dir.Sse,
              87,
              34 ]
        | 384, "right1" -> [ "left1", true, 121.59358535712317, Dir.W, -0.5, 39 ]
        | 384, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDoubleJump,
              121.59358535712317,
              Dir.E,
              120.5,
              51 ]
        | 385, "left3" ->
            [ "left2",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasDoubleJump,
              42.0,
              Dir.N,
              93.5,
              51 ]
        | 385, "left1" ->
            [ "left2",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasWalljump
                 && (PlayerData.instance.hasDash || PlayerData.instance.hasSuperDash)
              || PlayerData.instance.hasDoubleJump,
              23.0,
              Dir.S,
              93.5,
              51
              "left3", true, 65.0, Dir.S, 93.5, 9 ]
        | 385, "left2" -> [ "left3", true, 42.0, Dir.S, 93.5, 9 ]
        | 386, "bot1" ->
            [ "right1",
              PlayerData.instance.hasWalljump
              || PlayerData.instance.hasDoubleJump
              || PlayerData.instance.hasWalljump && PlayerData.instance.hasDash,
              72.50689622373861,
              Dir.Nee,
              140.5,
              144 ]
        | 388, "left1" ->
            [ "right1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              125.54282137979854,
              Dir.See,
              220.5,
              104 ]
        | 388, "right1" ->
            [ "left1",
              PlayerData.instance.hasWalljump && PlayerData.instance.hasDash
              || PlayerData.instance.hasDoubleJump,
              125.54282137979854,
              Dir.Nww,
              101.5,
              144 ]
        | _ -> []

    let sceneDoorsAll s =
        match s with
        | 6 -> [| "right1", 193.5, 68.0; "top1", 34.5, 93.5; "top2", 11, 80.5 |]
        | 7 ->
            [| "door_jiji", 252.00000610313353, 7.750000007857598
               "door_mapper", 154.798609883, 10.713547384000002
               "bot1", 185, -1.5
               "left1", 1.5, 49
               "right1", 263.5, 53
               "top1", 33.5, 72
               "room_divine", 72.143166, 10.8265
               "room_grimm", 82.75712, 10.8265
               "door_sly", 120.85837106458848, 10.64479010702712
               "door_bretta", 165.62100465999998, 10.87395055
               "door_station", 142.50163408847712, 10.645208875162702 |]
        | 9 -> [| "left1", 23, 21; "door_stagExit", 55.64, 5.74 |]
        | 10 -> [| "left1", 11.5, 7 |]
        | 11 -> [| "left1", 11.5, 7 |]
        | 12 -> [| "left1", 11.5, 7 |]
        | 13 -> [| "left1", 11, 5 |]
        | 14 -> [| "left1", 10, 5 |]
        | 15 -> [| "left1", 10, 5 |]
        | 16 -> [| "left1", 8.5, 5.5 |]
        | 17 -> [| "left1", 9.5, 7 |]
        | 18 -> [| "top1", 16, 16.5 |]
        | 19 -> [| "left1", 2, 5 |]
        | 20 -> [| "left1", 9.5, 7.5 |]
        | 21 -> [| "right1", 60.5, 7 |]
        | 22 -> [| "left1", 11, 7 |]
        | 25 -> [| "right1", 24.5, 7 |]
        | 26 -> [| "top1", 20.5, 15 |]
        | 27 -> [| "left1", -0.5, 11 |]
        | 28 -> [| "left1", 10, 8 |]
        | 29 -> [| "left1", 10, 8 |]
        | 30 -> [| "left1", 13.5, 9; "bot1", 44, 3 |]
        | 31 -> [| "top1", 43.5, 56; "top2", 135.5, 56 |]
        | 32 -> [| "left1", 1.5, 7; "left1 (1)", 1.5, 11 |]
        | 33 -> [| "left1", 1.5, 7; "left1 (1)", 1.5, 11 |]
        | 34 -> [| "left1", 1.5, 7; "left1 (1)", 1.5, 11 |]
        | 35 -> [| "bot1", 135, 2.5; "right1", 176, 9 |]
        | 36 -> [| "left1", 11.5, 7 |]
        | 37 ->
            [| "left1", -0.5, 9
               "right1", 100.5, 18
               "top1", 52.5, 25.5
               "top2", 52.5, 42.5 |]
        | 38 -> [| "left1", -0.5, 7; "right1", 90.5, 5; "door1", 46.47, 3.77 |]
        | 39 ->
            [| "left1", 0.5, 34
               "left2", -0.5, 11
               "right1", 30.5, 35
               "top1", 14, 72.5
               "bot1", 15, -0.5
               "right2", 30.5, 60 |]
        | 40 ->
            [| "left1", -0.5, 17
               "top1", 76.0, 30.5
               "door1", 85.050003, 2.82
               "right1", 160.500003, 25
               "door_Mender_House", 57.0999983215, 2.6674398262187005
               "door_charmshop", 144.059998, 10.74 |]
        | 41 -> [| "left1", -0.5, 7; "right1", 75, 8 |]
        | 42 -> [| "left1", -0.5, 6; "right1", 60.5, 6; "door1", 28.41, 29.66 |]
        | 43 ->
            [| "bot1", 20, 0.5
               "left1", 2.5, 83
               "left2", -0.5, 40.5
               "left3", -0.5, 20
               "right1", 43.5, 83
               "right2", 43.5, 44 |]
        | 44 -> [| "left1", -0.5, 22; "left2", -0.5, 6; "right1", 51.5, 24; "right2", 52.5, 9 |]
        | 45 -> [| "left1", -0.5, 9; "right1", 86.5, 5 |]
        | 46 -> [| "left1", -0.5, 4; "right1", 75.5, 4 |]
        | 50 -> [| "right1", 120.5, 13; "left1", -0.5, 19 |]
        | 51 -> [| "left1", -0.5, 13; "right1", 70.5, 11 |]
        | 52 -> [| "left1", -0.5, 13; "right1", 80.5, 14 |]
        | 53 ->
            [| "right1", 33.5, 35.5
               "right2", 33.5, 7
               "left1", -0.5, 34
               "left2", -0.5, 11 |]
        | 54 -> [| "left1", -0.5, 4; "right1", 60.5, 4 |]
        | 55 -> [| "bot1", 58, -0.5; "left1", -0.5, 10; "right1", 76.5, 15 |]
        | 56 -> [| "right2", 41.5, 9; "bot1", 21, -0.5; "right1", 41.5, 37 |]
        | 57 -> [| "left1", -0.5, 35; "left2", -0.5, 7; "right1", 50.5, 7; "top1", 18, 45.5 |]
        | 58 -> [| "left1", -0.5, 6; "right1", 100, 17; "top1", 9, 29.5 |]
        | 59 -> [| "bot1", 8, -0.5 |]
        | 60 -> [| "left1", -0.5, 8; "right1", 70.5, 8 |]
        | 61 -> [| "bot1", 15, -0.5; "left1", -0.5, 66; "left2", -0.5, 39; "right1", 30.5, 21 |]
        | 62 -> [| "left1", 1.5, 8 |]
        | 63 -> [| "right1", 69.5, 5 |]
        | 64 ->
            [| "left1", -0.5, 37
               "left2", -0.5, 10
               "right1", 45.5, 35
               "right2", 45.5, 10.5
               "top1", 21, 49.5 |]
        | 65 -> [| "right1", 70.5, 49; "bot1", 64, -0.5 |]
        | 66 -> [| "right1", 60.5, 45; "right2", 60.5, 5 |]
        | 67 -> [| "right1", 110.5, 4 |]
        | 68 -> [| "right1", 68.5, 4 |]
        | 69 -> [| "left1", -0.5, 7; "right1", 88.5, 7 |]
        | 70 -> [| "left1", -0.5, 4.5; "right1", 88.5, 4 |]
        | 71 -> [| "left1", -0.5, 4; "right1", 110.5, 5 |]
        | 72 -> [| "left1", -0.5, 10; "right1", 88.5, 4 |]
        | 73 -> [| "left1", -0.5, 9; "right1", 70.5, 41 |]
        | 74 -> [| "left1", -0.5, 11; "door_tram", 18.450000000000003, 9.989999999999998 |]
        | 75 -> [| "right1", 55.5, 11; "door_tram", 32.419999999999995, 9.989999999999998 |]
        | 76 -> [| "left1", -0.5, 9 |]
        | 77 -> [| "right1", 47.5, 7; "door_stagExit", 16.2, 5.74 |]
        | 78 -> [| "left1", -0.5, 4 |]
        | 79 -> [| "left1", -0.5, 161; "right1", 30.5, 161 |]
        | 80 -> [| "right1", 30.5, 6 |]
        | 81 -> [| "right1", 260.5, 28; "left1", -0.5, 45 |]
        | 82 -> [| "left1", -0.5, 61 |]
        | 83 -> [| "left1", 10.5, 7 |]
        | 84 -> [| "left1", 10.5, 7 |]
        | 85 -> [| "left2", 38, 5; "left1", 10.5, 52 |]
        | 86 -> [| "left2", 21, 141; "left1", 10.5, 7 |]
        | 87 -> [| "right1", 80.5, 33; "door1", 11.75, 3.71 |]
        | 88 -> [| "top1", 52, 35.5; "left1", -0.5, 18; "bot1", 95, -0.5 |]
        | 89 -> [| "top1", 11, 76.5; "bot1", 7, -0.5 |]
        | 90 ->
            [| "left1", -0.5, 11
               "right2", 150.5, 10
               "top1", 28, 70.5
               "right1", 148.5, 43 |]
        | 91 -> [| "right1", 150.5, 9; "bot1", 15, -0.5; "door1", 38.42376, 35.69 |]
        | 92 ->
            [| "bot1", 20, 102.5
               "top1", 9, 167.5
               "right2", 65.5, 109
               "right1", 63.5, 153.5
               "bot2", 32, 102.5
               "bot3", 55, 102.5 |]
        | 93 -> [| "right1", 65.5, 9; "bot1", 32, 3; "left1", -0.5, 9; "top1", 29, 35.5 |]
        | 94 ->
            [| "top2", 32, 113.5
               "bot1", 29, 32.5
               "top3", 54, 113.5
               "left2", 1, 39
               "top1", 20, 113.5 |]
        | 95 -> [| "left1", -0.5, 19; "right1", 100.5, 19 |]
        | 96 -> [| "top1", 31, 29.5; "left1", 1, 19 |]
        | 97 -> [| "top1", 7, 70.5; "right1", 90.5, 4; "bot1", 7, -0.5 |]
        | 98 -> [| "left1", -0.5, 23; "right2", 90.5, 10; "right1", 90.5, 23 |]
        | 99 ->
            [| "top1", 34, 86.5
               "right2", 50.5, 29
               "right1", 50.5, 75
               "bot1", 38, -0.5
               "left1", -0.5, 4 |]
        | 100 -> [| "left2", -0.5, 4; "left1", -0.5, 41; "right1", 80.5, 41; "bot1", 58, -0.5 |]
        | 101 -> [| "left2", -0.5, 4; "left1", -0.5, 19; "right1", 70.5, 43; "right2", 70.5, 9 |]
        | 104 -> [| "left1", -0.5, 99.5; "left3", -0.5, 4; "left2", -0.5, 49 |]
        | 105 -> [| "left1", -0.5, 8; "right1", 80.5, 6 |]
        | 106 -> [| "right1", 120.5, 19; "left1", -0.5, 9; "bot1", 78, -0.5 |]
        | 107 -> [| "left1", -0.5, 7; "door_stagExit", 31.83, 5.74 |]
        | 108 ->
            [| "bot1", 7, -0.5
               "right1", 65.5, 4
               "left1", -0.5, 21
               "left3", 43, 42
               "left2", 43, 57 |]
        | 109 -> [| "right1", 52.5, 59; "right2", 58.5, 40 |]
        | 110 -> [| "right2", 52.5, 50; "right1", 52.5, 66 |]
        | 111 -> [| "left2", -0.5, 85; "top1", 70, 90.5; "bot1", 6, 45.5 |]
        | 112 -> [| "left1", -0.5, 6; "right1", 78.5, 8; "top1", 6, 48.5 |]
        | 113 -> [| "top1", 52, 124.5; "bot1", 19, 32.5 |]
        | 114 -> [| "top2", 72, 48; "left1", -0.5, 6; "bot1", 71, -0.5; "top1", 12, 48 |]
        | 116 ->
            [| "left2", -0.5, 4
               "door_Ruin_Elevator", 68.70495, 32.68663
               "door_Ruin_House_03", 80.34, 6.62
               "door_Ruin_House_01", 122.26, 24.68
               "door_Ruin_House_02", 83.7, 39.63
               "right2", 150.5, 8
               "right1", 150.5, 51
               "left1", 1.5, 18 |]
        | 117 -> [| "top1", 10, 84.5; "bot1", 9, -0.5; "left1", -0.5, 57 |]
        | 118 ->
            [| "right2", 55.5, 6
               "left2", -0.5, 6
               "right1", 55.5, 32
               "left1", -0.5, 42
               "top1", 22.5, 49 |]
        | 119 -> [| "right1", 130.5, 10; "left1", -0.5, 7; "top1", 102, 20.5 |]
        | 120 -> [| "left1", -0.5, 7; "door_stagExit", 31.85, 5.64 |]
        | 121 -> [| "bot1", 8, -0.5 |]
        | 122 -> [| "left1", -0.5, 161; "right1", 30.5, 161 |]
        | 123 -> [| "left1", -0.5, 93; "right1", 30.5, 140; "right2", 30.5, 11 |]
        | 124 -> [| "right1", 48.5, 61 |]
        | 125 -> [| "bot1", 55, -0.5; "left1", 43.5, 61; "right1", 78.5, 61 |]
        | 127 -> [| "bot1", 53, -0.5 |]
        | 128 -> [| "left1", -0.5, 7; "right1", 170.5, 9.5 |]
        | 129 -> [| "right1", 45.5, 7; "left1", -0.5, 7 |]
        | 130 -> [| "left1", -0.5, 62; "right1", 46.5, 62; "right2", 46.5, 11 |]
        | 131 -> [| "bot1", 45, -0.5; "right1", 80.5, 18; "left1", -0.5, 30 |]
        | 132 -> [| "right1", 95.5, 33; "left1", -0.5, 6 |]
        | 134 -> [| "right1", 31.5, 13; "bot1", 15, -0.5; "top1", 22, 85.5 |]
        | 135 -> [| "bot1", 103, -0.5; "left1", -0.5, 17 |]
        | 136 -> [| "right1", 70.5, 44; "top1", 36, 58.5; "left1", -0.5, 14 |]
        | 137 -> [| "left1", -0.5, 39 |]
        | 138 -> [| "left1", -0.5, 11; "right1", 250.5, 14 |]
        | 139 -> [| "right1", 180.5, 13; "left1", -0.5, 14; "top1", 44, 25.5 |]
        | 140 ->
            [| "right1", 55.5, 40
               "bot1", 35, -0.5
               "top1", 41, 69
               "left1", -0.5, 33
               "right2", 55.5, 62 |]
        | 141 -> [| "right1", 32.5, 28; "left1", -0.5, 8 |]
        | 142 -> [| "right1", 220.5, 18; "left1", -0.5, 23 |]
        | 143 -> [| "left1", -0.5, 13 |]
        | 144 -> [| "right1", 57.5, 8; "door1", 36.95, 33.75 |]
        | 145 -> [| "right1", 47.5, 7; "door_stagExit", 16.09, 5.81 |]
        | 146 -> [| "right1", 80.5, 7; "left1", -0.5, 9 |]
        | 147 -> [| "left1", -0.5, 14; "right1", 90.5, 14; "bot1", 67, -0.5 |]
        | 148 -> [| "bot1", 7, -0.5; "bot2", 163, -0.5; "right1", 180.5, 13 |]
        | 149 -> [| "top1", 9, 50.5; "right1", 90.5, 14; "bot1", 21, -0.5; "left1", -0.5, 14 |]
        | 150 -> [| "left1", -0.5, 104; "bot1", 30, -0.5; "top1", 8, 120.5 |]
        | 151 -> [| "left1", -0.5, 7; "right1", 150.5, 10 |]
        | 152 -> [| "left1", -0.5, 11 |]
        | 153 -> [| "right1", 90.5, 11; "left1", -0.5, 14.5 |]
        | 154 -> [| "left1", -0.5, 22; "door_SlugShrine", 77.98, 14.76; "right1", 110.5, 19.5 |]
        | 155 -> [| "left2", -0.5, 18; "left1", -0.5, 59 |]
        | 156 -> [| "right1", 85.5, 15; "left1", -0.5, 15 |]
        | 157 -> [| "top3", 94, 32.5; "top1", 28, 32.5; "right1", 100.5, 15; "left1", -0.5, 15 |]
        | 158 -> [| "right1", 36.5, 116; "bot1", 21, -0.5; "top1", 14, 133.5 |]
        | 159 -> [| "left1", -0.5, 4; "top1", 68, 40.5; "bot1", 7, -0.5 |]
        | 160 -> [| "door1", 103.16, 3.73; "left1", -0.5, 5 |]
        | 161 -> [| "right1", 120.5, 17; "left1", -0.5, 4 |]
        | 162 -> [| "left1", -0.5, 4 |]
        | 163 -> [| "left1", -0.5, 7 |]
        | 164 -> [| "right1", 70.5, 102 |]
        | 165 -> [| "right1", 60.5, 42; "left1", -0.5, 42; "left2", -0.5, 6; "left3", -0.5, 19 |]
        | 166 -> [| "right1", 47.5, 7; "door_stagExit", 16.13, 5.8 |]
        | 167 -> [| "right1", 130.5, 17; "left1", -0.5, 15; "bot1", 57, -0.5 |]
        | 168 ->
            [| "right2", 36.5, 55
               "right1", 36.5, 72
               "top1", 29, 76.5
               "left1", -0.5, 12 |]
        | 169 -> [| "bot1", 8, -0.5; "right1", 75.5, 14 |]
        | 170 ->
            [| "left2", -0.5, 59
               "right2", 36.5, 10
               "left1", -0.5, 10
               "top1", 13, 160.5
               "right1", 36.5, 59 |]
        | 171 -> [| "left1", -0.5, 14; "right1", 70.5, 5 |]
        | 172 -> [| "right1", 30.5, 13; "left1", -0.5, 60; "left2", -0.5, 11 |]
        | 173 -> [| "left1", -0.5, 11; "right1", 70.5, 12 |]
        | 174 -> [| "right1", 70.5, 61; "right2", 70.5, 10; "bot1", 6, -0.5 |]
        | 175 -> [| "top1", 7, 70.5; "left1", -0.5, 41; "left2", -0.5, 15; "right1", 31.5, 5 |]
        | 176 -> [| "bot1", 91, -0.5; "left1", -0.5, 8 |]
        | 177 -> [| "top1", 24, 116.5; "left3", -0.5, 5; "left2", 1.5, 59 |]
        | 178 ->
            [| "bot3", 60, -0.5
               "bot2", 52, -0.5
               "top1", 35, 41.5
               "right1", 175.5, 10
               "bot1", 44, -0.5 |]
        | 179 ->
            [| "right1", 61.5, 88
               "left1", 7, 10
               "top1", 29, 120.5
               "top2", 33.5, 120.5
               "top3", 38, 120.5 |]
        | 182 -> [| "left1", -0.5, 34; "bot1", 8, -0.5; "right1", 45.5, 37 |]
        | 183 -> [| "bot1", 6, -0.5; "right1", 150.5, 8; "top1", 40, 60.5 |]
        | 184 -> [| "top1", 50, 25.5; "left1", -0.5, 20 |]
        | 185 -> [| "left1", -0.5, 31; "right1", 63.5, 31 |]
        | 186 -> [| "left1", -0.5, 11; "right1", 150.5, 13 |]
        | 187 -> [| "right2", 90.5, 27; "right1", 90.5, 63 |]
        | 188 ->
            [| "top1", 12, 35.5
               "top2", 95, 26
               "right1", 168.5, 6
               "right1 (1)", 168.5, 10 |]
        | 189 -> [| "left1", -0.5, 8 |]
        | 190 -> [| "left2", -0.5, 5; "left1", -0.5, 20 |]
        | 191 -> [| "bot1", 8, -0.5; "right1", 94.5, 32 |]
        | 192 -> [| "bot1", 17.5, -0.5; "top1", 20, 140.5 |]
        | 193 -> [| "left1", -0.5, 4 |]
        | 194 -> [| "left1", -0.5, 4 |]
        | 195 -> [| "right1", 60.5, 8; "left1", -0.5, 63 |]
        | 196 -> [| "right1", 47.5, 7 |]
        | 197 ->
            [| "right2", 33.5, 13
               "left1", -0.5, 38
               "top1", 20, 80.5
               "right1", 33.5, 67 |]
        | 198 ->
            [| "right1", 30.5, 95
               "left3", -0.5, 9
               "left1", -0.5, 95
               "right2", 31.5, 4
               "left2", -0.5, 64 |]
        | 199 -> [| "left1", -0.5, 12; "right1", 100.5, 9 |]
        | 200 ->
            [| "left1", -0.5, 84
               "right2", 40.5, 6
               "right1", 40.5, 57
               "left2", -0.5, 67 |]
        | 201 -> [| "right2", 73.5, 30; "left1", -0.5, 48; "right1", 73.5, 60 |]
        | 202 -> [| "left1", -0.5, 8; "top1", 6, 25.5; "right1", 150.5, 14 |]
        | 203 -> [| "bot1", 7, -0.5; "top1", 49, 44.5 |]
        | 204 -> [| "right1", 52.5, 15; "left1", -0.5, 62; "left2", -0.5, 10 |]
        | 205 ->
            [| "left3", -0.5, 36
               "bot1", 7, -0.5
               "left2", -0.5, 6
               "left1", -0.5, 60
               "right1", 30.5, 64 |]
        | 206 -> [| "top1", 9, 26; "right1", 75.5, 7 |]
        | 207 -> [| "right1", 31.5, 84; "left1", -0.5, 4; "bot1", 7, -0.5 |]
        | 208 -> [| "left1", -0.5, 28; "right1", 100.5, 6 |]
        | 210 -> [| "left1", -0.5, 13; "top1", 30, 30.5; "right1", 100.5, 13 |]
        | 211 -> [| "right1", 100.5, 16; "left1", -0.5, 20 |]
        | 212 -> [| "left1", -0.5, 16; "right1", 100.5, 16 |]
        | 213 ->
            [| "right1", 31.5, 67
               "left1", -0.5, 62
               "top1", 21, 105.5
               "left3", -0.5, 10
               "left2", -0.5, 38 |]
        | 214 -> [| "right1", 75.5, 9; "left1", -0.5, 4 |]
        | 215 -> [| "right1", 75.5, 17 |]
        | 216 -> [| "bot1", 32, -0.5 |]
        | 217 -> [| "right1", 160.5, 14; "left1", -0.5, 12; "top1", 80, 26.5 |]
        | 218 -> [| "right1", 65.5, 6 |]
        | 219 -> [| "right1", 109.5, 7; "left1", -0.5, 48 |]
        | 220 -> [| "top1", 19, 35.5; "right1", 187.5, 14; "door_stagExit", 156.3, 12.63 |]
        | 222 -> [| "bot1", 53, -0.5; "right1", 95.5, 41; "door1", 21.74, 37.82 |]
        | 223 -> [| "door1", 43.45, 6.77; "right1", 80.5, 23; "left1", -0.5, 8 |]
        | 224 ->
            [| "bot1", 62, -0.5
               "right1", 95.5, 95
               "right2", 95.5, 28
               "door1", 38.021, 93.644 |]
        | 225 -> [| "right1", 87.5, 9 |]
        | 226 -> [| "right1", 40.5, 116 |]
        | 227 -> [| "bot1", 68, -0.5; "left1", -0.5, 7 |]
        | 228 -> [| "top1", 51, 195.5 |]
        | 230 ->
            [| "right2", 140.5, 92
               "right3", 140.5, 8
               "right1", 140.5, 144
               "right4", 140.5, 32 |]
        | 231 ->
            [| "right1", 241.5, 49.5
               "bot2", 221.5, -2
               "left1", -0.5, 25
               "bot1", 188, -0.5
               "left2", 3, 54
               "door1", 122.54, 5.64 |]
        | 233 -> [| "right1", 180.5, 58; "door_stagExit", 16.13, 5.71 |]
        | 234 -> [| "right1", 85.5, 49; "left1", -0.5, 44 |]
        | 235 -> [| "left1", -0.5, 49 |]
        | 236 -> [| "left1", -0.5, 5 |]
        | 237 -> [| "bot1", 98, 1.5; "left1", -0.5, 14; "right1", 129.5, 6; "top1", 109, 26 |]
        | 239 -> [| "right1", 110.5, 6; "left1", -0.5, 6 |]
        | 240 ->
            [| "left2", 2, 79
               "right2", 45.5, 55
               "right1", 33.5, 77
               "left1", -0.5, 4
               "bot1", 34.5, -0.5
               "left3", 1.5, 55 |]
        | 241 -> [| "left1", -0.5, 8; "right1", 110.5, 8; "top1", 8, 30.5 |]
        | 242 -> [| "right1", 45, 10 |]
        | 243 -> [| "left1", 13.5, 8 |]
        | 244 -> [| "left1", -0.5, 7; "door_stagExit", 31.78, 5.74 |]
        | 245 -> [| "top2", 130.5, 30.5; "top1", 72.5, 30.5; "left1", -0.5, 12 |]
        | 246 -> [| "bot1", 20.5, -0.5; "door_Mansion", 62.95, 2.74 |]
        | 247 -> [| "right1", 70.5, 6 |]
        | 248 -> [| "left1", -0.5, 49; "bot1", 14, -0.5 |]
        | 249 -> [| "left1", -0.5, 28; "right1", 160.5, 4; "top2", 119, 36.5; "top1", 20, 36.5 |]
        | 250 -> [| "bot1", 16, 0; "top1", 12, 80.5; "right1", 45.5, 41 |]
        | 251 ->
            [| "right1", 36.5, 56
               "top1", 22, 85.5
               "left3", -0.5, 7
               "left1", -0.5, 67
               "left2", -0.5, 45 |]
        | 252 ->
            [| "right1", 35.5, 33
               "top1", 15, 74.5
               "left1", -0.5, 69
               "bot1", 7, -0.5
               "left2", -0.5, 22 |]
        | 253 -> [| "left1", -0.5, 16; "right1", 150.5, 11 |]
        | 254 -> [| "left1", -0.5, 7.0; "right1", 120.5, 6.0 |]
        | 255 -> [| "right1", 187.5, 11; "left1", -0.5, 11; "bot1", 32, -0.5 |]
        | 256 -> [| "bot1", 13, -0.5; "right1", 43.5, 22; "top1", 17, 65.5 |]
        | 257 -> [| "top1", 86.5, 50; "right1", 100.5, 16; "bot1", 31, -0.5 |]
        | 258 -> [| "top1", 22, 35.5 |]
        | 259 -> [| "right1", 75.5, 12; "left1", -0.5, 12 |]
        | 260 -> [| "right1", 65.5, 12; "left1", -0.5, 12; "top1", 56, 30.5 |]
        | 262 -> [| "right1", 60.5, 7; "left1", -0.5, 7 |]
        | 263 ->
            [| "bot1", 39, 59.5
               "right2", 75.5, 64
               "right1", 75.5, 196
               "left2", -0.5, 120
               "left1", -0.5, 187
               "left3", -0.5, 64 |]
        | 264 ->
            [| "top1", 32.5, 40.5
               "left1", -0.5, 13
               "right1", 180.5, 28
               "right2", 180.5, 6 |]
        | 265 -> [| "left1", -0.5, 8 |]
        | 266 -> [| "top1", 6, 130.5; "left1", -0.5, 21 |]
        | 267 -> [| "door1", 81.2, 47.56; "left1", -0.5, 49; "bot1", 20, -0.5 |]
        | 268 -> [| "right1", 45.5, 57; "right2", 45.5, 13; "left1", -0.5, 4 |]
        | 269 -> [| "right1", 180.5, 11; "left1", -0.5, 9 |]
        | 270 -> [| "left1", -0.5, 20 |]
        | 271 -> [| "bot1", 56, -0.5 |]
        | 272 -> [| "right1", 100.5, 8; "left1", -0.5, 9 |]
        | 273 ->
            [| "right1", 193.5, 50
               "left1", -3.5, 40
               "bot1", 166, -0.5
               "bot2", 15.5, -2 |]
        | 274 -> [| "left1", -0.5, 49 |]
        | 275 -> [| "right1", 43.5, 23 |]
        | 276 -> [| "top1", 38, 65.5; "bot1", 32, -0.5 |]
        | 277 -> [| "right1", 60.5, 20; "left1", 1, 21; "bot2", 32, -0.5; "bot1", 9, -0.5 |]
        | 278 ->
            [| "right1", 60.5, 66
               "right2", 60.5, 37
               "bot1", 25, -0.5
               "top2", 32, 86
               "top1", 9, 83.5 |]
        | 279 -> [| "right1", 31.5, 45; "left1", -0.5, 67; "left2", -0.5, 36 |]
        | 280 -> [| "left1", -0.5, 48; "left2", -0.5, 15; "right1", 60.5, 4; "top1", 48, 113.5 |]
        | 281 -> [| "left1", -0.5, 7; "door_stagExit", 31.83, 5.73 |]
        | 282 ->
            [| "door2", 19.82, 94.49
               "right1", 75.5, 139
               "right2", 75.5, 89
               "right3", 75.5, 14
               "door1", 32.87, 117.51 |]
        | 283 ->
            [| "bot1", 14, -0.5
               "bot2", 63, -0.5
               "left1", 6.5, 42
               "right1", 80.5, 4
               "left1 (1)", 6.5, 46
               "left1 (2)", 6.5, 50
               "left1 (3)", 6.5, 54 |]
        | 284 -> [| "bot1", 73, -0.5; "left1", -0.5, 8 |]
        | 285 -> [| "bot1", 14, -0.5; "left1", -0.5, 8; "right1", 31.5, 5; "top1", 16, 62.5 |]
        | 286 -> [| "bot1", 224, -0.5; "right1", 246, 18; "left2", 121, 5; "left1", 121, 23 |]
        | 287 -> [| "right1", 128.5, 21; "right2", 128.5, 5 |]
        | 288 -> [| "left1", -0.5, 6; "top1", 20, 159.5; "right1", 120.5, 6 |]
        | 289 -> [| "right2", 125.5, 5; "right1", 125.5, 53 |]
        | 290 -> [| "left1", -0.5, 5 |]
        | 291 -> [| "bot1", 73, -0.5; "top1", 14, 45.5; "top2", 73, 45.5 |]
        | 292 -> [| "left1", -0.5, 16; "top1", 98, 50.5; "right1", 150.5, 28 |]
        | 293 -> [| "left1", -0.5, 53; "bot1", 17, -0.5; "top1", 35, 110.5 |]
        | 294 -> [| "left1", -0.5, 16 |]
        | 295 -> [| "bot1", 44, -0.5; "top1", 19, 32.5; "left1", -0.5, 4; "right1", 82.5, 4 |]
        | 296 -> [| "bot1", 8, -0.5 |]
        | 297 ->
            [| "door1", 195.93, 38.72
               "right1", 210.5, 8
               "top1", 73, 70.5
               "left1", -0.5, 57 |]
        | 298 -> [| "right1", 150.5, 17 |]
        | 299 -> [| "left2", -0.5, 6; "right1", 120.5, 88; "left1", -0.5, 91 |]
        | 300 -> [| "top1", 7, 146.5; "bot1", 26, -0.5; "left1", -0.5, 124 |]
        | 301 -> [| "left1", -0.5, 116; "bot1", 7, -0.5; "right1", 40.5, 116 |]
        | 302 -> [| "top1", 39, 60.5 |]
        | 303 -> [| "left1", 18.5, 16 |]
        | 304 -> [| "left1", 5.5, 59 |]
        | 305 -> [| "left1", 12.5, 14 |]
        | 306 -> [| "top1", 23, 85.5; "bot1", 36, -0.5; "right1", 49.5, 37 |]
        | 307 -> [| "bot1", 12, -0.5; "bot2", 71, -0.5; "right1", 110.5, 17; "top1", 16, 32.5 |]
        | 308 ->
            [| "top1", 9, 150.5
               "right1", 80.5, 132
               "right2", 80.5, 17
               "left2", -0.5, 18
               "left1", -0.5, 101
               "top2", 35.5, 150.5 |]
        | 309 ->
            [| "right2", 45.5, 127
               "left2", -0.5, 94
               "right1", 45.5, 7
               "left1", -0.5, 7 |]
        | 310 ->
            [| "right1", 351.5, 16
               "top1", 134, 43.5
               "door1", 329.22, 14.71
               "left1", -0.5, 14
               "bot1", 219.5, 5 |]
        | 311 ->
            [| "left2", 1.5, 32
               "left1", -0.5, 171
               "right1", 90.5, 65
               "bot2", 37, -0.5
               "bot1", 6, -0.5 |]
        | 312 -> [| "top1", 6, 40.5; "right1", 120.5, 10 |]
        | 313 -> [| "right1", 171.5, 14; "left1", -0.5, 15; "bot1", 85, -0.5 |]
        | 314 -> [| "left1", -0.5, 4 |]
        | 315 ->
            [| "bot1", 37, 38.5
               "top1", 78, 130.5
               "left1", -0.5, 87
               "right1", 110.5, 120 |]
        | 316 -> [| "right1", 120.5, 12; "left1", -0.5, 12 |]
        | 317 -> [| "bot1", 19.5, -0.5 |]
        | 318 -> [| "door1", 136.68, 6.86; "top2", 150, 70.5; "left1", 49, 8 |]
        | 319 -> [| "right1", 61, 8; "top1", 9, 70.5 |]
        | 320 -> [| "left1", -0.5, 5 |]
        | 321 -> [| "bot1", 42, 1.5; "left1", -0.5, 4 |]
        | 322 -> [| "left1", 19, 294 |]
        | 323 -> [| "top1", 37, 41.5; "right2", 110.5, 15; "bot1", 65, -0.5 |]
        | 324 -> [| "left2", 16, 7.5; "left1", -0.5, 83 |]
        | 326 -> [| "right1", 108.5, 12 |]
        | 327 ->
            [| "left1", -0.5, 133
               "right1", 30.5, 161
               "left3", -0.5, 11
               "left2", -0.5, 51
               "right2", 30.5, 51 |]
        | 328 -> [| "bot1", 9, -0.5; "right1", 170.5, 23 |]
        | 329 ->
            [| "top1", 47, 22.5
               "bot1", 14, 0
               "bot2", 62.5, 7
               "door_tram_arrive", 80.36, 9.952386 |]
        | 330 -> [| "left1", -0.5, 10; "door_tram_arrive", 32.34, 9.99 |]
        | 331 -> [| "top1", 140, 25.5; "right1", 150.5, 10; "door_tram_arrive", 44.42, 10.09 |]
        | 332 -> [| "bot1", 59, -0.5; "top1", 56, 90.5; "right1", 100.5, 21; "left1", -0.5, 9 |]
        | 333 -> [| "right1", 200.5, 18; "left1", -0.5, 18 |]
        | 334 ->
            [| "bot1", 27, -0.5
               "left1", 0.5, 140
               "left1 extra", 0.5, 144
               "left3", -0.5, 6
               "right2", 100.5, 6
               "top1", 90, 270.5 |]
        | 335 -> [| "right1", 110, 92 |]
        | 336 ->
            [| "right3", 260.5, 57
               "right1", 260.5, 26
               "left1", -0.5, 26
               "right2", 81.50000081923, 90.9999970592716 |]
        | 337 -> [| "left2", -0.5, 42; "left1", -0.5, 11 |]
        | 338 -> [| "right1", 180.5, 16 |]
        | 339 -> [| "top1", 11, 110.5 |]
        | 340 -> [| "right1", 100.5, 9; "left1", -0.5, 8 |]
        | 341 -> [| "top1", 163, 37 |]
        | 342 -> [| "right1", 200.5, 12; "left1", -0.5, 18 |]
        | 343 ->
            [| "right1", 160.5, 32
               "left1", -0.5, 28.5
               "bot1", 84, -0.5
               "bot2", 118.5, -0.5 |]
        | 344 -> [| "top1", 83, 70.5; "top2", 118.5, 70.5 |]
        | 345 -> [| "right1", 170.5, 247 |]
        | 346 -> [| "left1", -0.5, 7; "door_stagExit", 35.6, 5.64 |]
        | 347 -> [| "left1", 8, 8 |]
        | 348 -> [| "left1", 4.5, 45 |]
        | 349 -> [| "top1", 77, 47; "right1", 152.5, 41; "bot1", 54, -0.5; "left1", -0.5, 14 |]
        | 350 ->
            [| "top3", 10, 45.5
               "bot1", 9, -0.5
               "top2", 171, 45.5
               "top1", 65, 45.5
               "bot2", 220, -0.5 |]
        | 351 -> [| "left1", 44, 5 |]
        | 352 ->
            [| "bot1", 146, -0.5
               "left2", -0.5, 9
               "left1", -0.5, 34
               "right1", 154.5, 37 |]
        | 353 -> [| "right2", 130.5, 9; "left1", -0.5, 23; "right1", 130.5, 34 |]
        | 354 -> [| "bot2", 104, 3; "right1", 120.5, 10; "bot1", 8, -0.5 |]
        | 356 -> [| "right1", 140.5, 18; "top1", 93, 50.5 |]
        | 357 ->
            [| "top1", 11, 100.5
               "right1", 110.5, 50
               "left1", -0.5, 21
               "door1", 81.11, 86.68
               "right2", 110.5, 17 |]
        | 358 -> [| "left1", -0.5, 7; "top1", 25, 60.5; "left2", 0, 55 |]
        | 359 -> [| "right1", 50.5, 20; "left1", 1, 7 |]
        | 360 -> [| "right1", 80.5, 6 |]
        | 362 -> [| "left2", -0.5, 17; "left1", -0.5, 48 |]
        | 363 -> [| "bot1", 8, -0.5; "bot2", 249, -0.5 |]
        | 364 -> [| "top1", 39, 25 |]
        | 365 -> [| "right1", 200.5, 24; "left1", 0, 21; "top1", 107, 140 |]
        | 366 -> [| "left1", -0.5, 24 |]
        | 367 ->
            [| "top1", 54, 140.5
               "bot1", 10, -0.5
               "right1", 100.5, 64
               "left1", -0.5, 98
               "left2", -0.5, 35 |]
        | 368 -> [| "top1", 19, 78.5; "right2", 200.5, 24 |]
        | 369 ->
            [| "left1", 50.5, 115
               "right1", 156.5, 115
               "right2", 156.5, 19
               "left2", 48.5, 19 |]
        | 370 -> [| "left1", -0.5, 8; "bot1", 19, -0.5; "top1", 19, 75.5 |]
        | 371 -> [| "top1", 31, 140.5; "bot1", 51, -0.5 |]
        | 372 -> [| "right1", 154, 21; "left1", -0.5, 9 |]
        | 373 -> [| "right1", 150.5, 10 |]
        | 374 -> [| "door2", 128.81, 16.8 |]
        | 375 -> [| "right1", 74.5, 136; "bot1", 34, 132 |]
        | 376 ->
            [| "right1", 175.5, 151
               "left3", 98.5, 215
               "left1", -0.5, 231
               "left2", -0.5, 136 |]
        | 377 -> [| "bot1", 17, 68.5; "right1", 200.5, 81 |]
        | 378 -> [| "right1", 70.5, 115; "left1", -0.5, 19; "right2", 70.5, 19 |]
        | 379 -> [| "left1", 135.5, 115; "left2", 134.5, 19 |]
        | 380 -> [| "right1", 120.5, 34; "bot1", 80, -0.5 |]
        | 381 -> [| "top1", 78, 44.5; "right1", 300.5, 13 |]
        | 382 -> [| "top1", 18, 168.5; "left1", 87, 34 |]
        | 383 -> [| "bot1", 10, 154 |]
        | 384 -> [| "right2", 120.5, 9; "left1", -0.5, 39; "right1", 120.5, 51 |]
        | 385 -> [| "left2", 93.5, 51; "left3", 93.5, 9; "left1", 93.5, 74 |]
        | 386 -> [| "right1", 140.5, 144; "top1", 41, 150.5; "bot1", 77, 109 |]
        | 387 ->
            [| "right2", 140.5, 96
               "right3", 140.5, 73
               "left1", -0.5, 101
               "top1", 77, 110.5 |]
        | 388 -> [| "right1", 220.5, 104; "left1", 101.5, 144; "left2", 100.5, 96 |]
        | 389 -> [| "left1", -0.5, 28 |]
        | 390 -> [| "left1", 11.5, 7 |]
        | 391 -> [| "left1", 11, 7 |]
        | 405 -> [| "left1", 5, 17 |]
        | 408 -> [| "right1", 246, 9; "left1", 6.5, 9 |]
        | 423 -> [| "right1", 164.5, 88; "door1", 139.71, 55.8 |]
        | 458 -> [| "left1", -0.5, 33; "right1", 149, 5 |]
        | 471 -> [| "top1", 4.5, 108.5; "left1", 28.5, 5 |]
        | _ -> [||]

    let reachability s =
        match s with
        | 6 -> if PlayerData.instance.mapDirtmouth then Always else Never
        | 7 -> if PlayerData.instance.mapDirtmouth then Always else Never
        | 9 -> Passthru
        | 10 -> Passthru
        | 11 -> Passthru
        | 12 -> Passthru
        | 13 -> Passthru
        | 14 -> Passthru
        | 15 -> Passthru
        | 16 -> Passthru
        | 17 -> Passthru
        | 18 -> Passthru
        | 19 -> Passthru
        | 20 -> Passthru
        | 21 -> Passthru
        | 22 -> Passthru
        | 23 -> Passthru
        | 24 -> Passthru
        | 25 -> Passthru
        | 26 -> Passthru
        | 27 -> Passthru
        | 28 -> Passthru
        | 29 -> Passthru
        | 30 -> Passthru
        | 31 -> Passthru
        | 32 -> Passthru
        | 33 -> Passthru
        | 34 -> Passthru
        | 35 -> Passthru
        | 36 -> Passthru
        | 37 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 38 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 39 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 40 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 41 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 42 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 43 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 44 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 45 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 46 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 50 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 51 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 52 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 53 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 54 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 55 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 56 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 57 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 58 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 59 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 60 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 61 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 62 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 63 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 64 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 65 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 66 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 67 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 68 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 69 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 70 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 71 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 72 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 73 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 74 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 75 ->
            if PlayerData.instance.mapRestingGrounds then
                Always
            else
                Never
        | 76 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 77 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 78 -> if PlayerData.instance.mapCrossroads then Always else Never
        | 79 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 80 -> if PlayerData.instance.mapCity then Visited else Never
        | 81 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 82 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 83 -> Passthru
        | 84 -> Passthru
        | 85 -> Passthru
        | 86 -> if PlayerData.instance.mapCity then Visited else Never
        | 87 -> if PlayerData.instance.mapCity then Visited else Never
        | 88 -> if PlayerData.instance.mapCity then Always else Never
        | 89 -> if PlayerData.instance.mapCity then Always else Never
        | 90 -> if PlayerData.instance.mapCity then Always else Never
        | 91 -> if PlayerData.instance.mapCity then Visited else Never
        | 92 -> if PlayerData.instance.mapCity then Always else Never
        | 93 -> if PlayerData.instance.mapCity then Always else Never
        | 94 -> if PlayerData.instance.mapCity then Always else Never
        | 95 -> if PlayerData.instance.mapCity then Always else Never
        | 96 -> if PlayerData.instance.mapCity then Always else Never
        | 97 -> if PlayerData.instance.mapCity then Always else Never
        | 98 -> if PlayerData.instance.mapCity then Visited else Never
        | 99 -> if PlayerData.instance.mapCity then Visited else Never
        | 100 -> if PlayerData.instance.mapCity then Visited else Never
        | 101 -> if PlayerData.instance.mapCity then Visited else Never
        | 104 -> if PlayerData.instance.mapCity then Visited else Never
        | 105 -> if PlayerData.instance.mapCity then Always else Never
        | 106 -> if PlayerData.instance.mapCity then Always else Never
        | 107 -> if PlayerData.instance.mapCity then Always else Never
        | 108 -> if PlayerData.instance.mapCity then Visited else Never
        | 109 -> if PlayerData.instance.mapCity then Visited else Never
        | 110 -> if PlayerData.instance.mapCity then Visited else Never
        | 111 -> if PlayerData.instance.mapCity then Visited else Never
        | 112 -> if PlayerData.instance.mapCity then Visited else Never
        | 113 -> if PlayerData.instance.mapCity then Visited else Never
        | 114 -> if PlayerData.instance.mapCity then Visited else Never
        | 116 -> if PlayerData.instance.mapCity then Visited else Never
        | 117 -> if PlayerData.instance.mapCity then Visited else Never
        | 118 -> if PlayerData.instance.mapCity then Visited else Never
        | 119 -> if PlayerData.instance.mapCity then Visited else Never
        | 120 -> if PlayerData.instance.mapCity then Visited else Never
        | 121 -> if PlayerData.instance.mapCity then Visited else Never
        | 122 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 123 -> if PlayerData.instance.mapCity then Visited else Never
        | 124 -> if PlayerData.instance.mapCity then Visited else Never
        | 125 -> if PlayerData.instance.mapCity then Visited else Never
        | 127 -> if PlayerData.instance.mapCity then Visited else Never
        | 128 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 129 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 130 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 131 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 132 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 134 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 135 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 136 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 137 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 138 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 139 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 140 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 141 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 142 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 143 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 144 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 145 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 146 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 147 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 148 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 149 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 150 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 151 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 152 ->
            if PlayerData.instance.mapRoyalGardens then
                Always
            else
                Never
        | 153 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 154 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 155 -> if PlayerData.instance.mapCliffs then Visited else Never
        | 156 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 157 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 158 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 159 -> if PlayerData.instance.mapGreenpath then Always else Never
        | 160 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 161 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 162 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 163 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 164 -> if PlayerData.instance.mapGreenpath then Visited else Never
        | 165 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 166 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 167 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 168 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 169 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 170 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 171 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 172 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 173 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 174 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 175 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 176 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 177 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 178 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 179 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 182 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 183 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 184 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 185 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 186 ->
            if PlayerData.instance.mapFungalWastes then
                Always
            else
                Never
        | 187 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 188 -> if PlayerData.instance.mapDeepnest then Always else Never
        | 189 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 190 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 191 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 192 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 193 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 194 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 195 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 196 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 197 -> if PlayerData.instance.mapFogCanyon then Always else Never
        | 198 -> if PlayerData.instance.mapFogCanyon then Always else Never
        | 199 -> if PlayerData.instance.mapFogCanyon then Visited else Never
        | 200 ->
            if PlayerData.instance.mapRoyalGardens then
                Always
            else
                Never
        | 201 ->
            if PlayerData.instance.mapRoyalGardens then
                Always
            else
                Never
        | 202 ->
            if PlayerData.instance.mapRoyalGardens then
                Always
            else
                Never
        | 203 ->
            if PlayerData.instance.mapRoyalGardens then
                Always
            else
                Never
        | 204 ->
            if PlayerData.instance.mapRoyalGardens then
                Always
            else
                Never
        | 205 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 206 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 207 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 208 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 210 -> if PlayerData.instance.mapFogCanyon then Visited else Never
        | 211 -> if PlayerData.instance.mapFogCanyon then Always else Never
        | 212 -> if PlayerData.instance.mapFogCanyon then Always else Never
        | 213 -> if PlayerData.instance.mapFogCanyon then Always else Never
        | 214 -> if PlayerData.instance.mapFogCanyon then Always else Never
        | 215 -> if PlayerData.instance.mapFogCanyon then Visited else Never
        | 216 -> if PlayerData.instance.mapFogCanyon then Visited else Never
        | 217 ->
            if PlayerData.instance.mapRoyalGardens then
                Always
            else
                Never
        | 218 -> if PlayerData.instance.mapFogCanyon then Visited else Never
        | 219 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 220 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 222 -> if PlayerData.instance.mapFogCanyon then Visited else Never
        | 223 -> if PlayerData.instance.mapFogCanyon then Always else Never
        | 224 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 225 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 226 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 230 -> if PlayerData.instance.mapCliffs then Visited else Never
        | 231 -> if PlayerData.instance.mapCliffs then Visited else Never
        | 233 -> if PlayerData.instance.mapCliffs then Visited else Never
        | 234 -> if PlayerData.instance.mapCliffs then Visited else Never
        | 235 -> if PlayerData.instance.mapCliffs then Visited else Never
        | 236 -> if PlayerData.instance.mapCliffs then Visited else Never
        | 237 ->
            if PlayerData.instance.mapRestingGrounds then
                Always
            else
                Never
        | 239 ->
            if PlayerData.instance.mapRestingGrounds then
                Always
            else
                Never
        | 240 ->
            if PlayerData.instance.mapRestingGrounds then
                Always
            else
                Never
        | 241 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 242 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 243 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 244 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 245 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 246 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 247 ->
            if PlayerData.instance.mapRestingGrounds then
                Visited
            else
                Never
        | 248 -> if PlayerData.instance.mapMines then Always else Never
        | 249 -> if PlayerData.instance.mapMines then Always else Never
        | 250 -> if PlayerData.instance.mapMines then Always else Never
        | 251 -> if PlayerData.instance.mapMines then Visited else Never
        | 252 -> if PlayerData.instance.mapMines then Always else Never
        | 253 -> if PlayerData.instance.mapMines then Visited else Never
        | 254 -> if PlayerData.instance.mapMines then Visited else Never
        | 255 -> if PlayerData.instance.mapMines then Visited else Never
        | 256 -> if PlayerData.instance.mapMines then Always else Never
        | 257 -> if PlayerData.instance.mapMines then Always else Never
        | 258 -> if PlayerData.instance.mapMines then Visited else Never
        | 259 -> if PlayerData.instance.mapMines then Visited else Never
        | 260 -> if PlayerData.instance.mapMines then Visited else Never
        | 262 -> if PlayerData.instance.mapMines then Always else Never
        | 263 -> if PlayerData.instance.mapMines then Visited else Never
        | 264 -> if PlayerData.instance.mapMines then Visited else Never
        | 265 -> if PlayerData.instance.mapMines then Visited else Never
        | 266 -> if PlayerData.instance.mapMines then Visited else Never
        | 267 -> if PlayerData.instance.mapMines then Visited else Never
        | 268 -> if PlayerData.instance.mapMines then Always else Never
        | 269 -> if PlayerData.instance.mapMines then Always else Never
        | 270 -> if PlayerData.instance.mapMines then Visited else Never
        | 271 -> if PlayerData.instance.mapMines then Visited else Never
        | 272 -> if PlayerData.instance.mapCrossroads then Visited else Never
        | 273 -> if PlayerData.instance.mapMines then Visited else Never
        | 274 -> if PlayerData.instance.mapMines then Visited else Never
        | 275 -> if PlayerData.instance.mapMines then Visited else Never
        | 276 -> if PlayerData.instance.mapMines then Visited else Never
        | 277 ->
            if PlayerData.instance.mapFungalWastes then
                Visited
            else
                Never
        | 278 -> if PlayerData.instance.mapDeepnest then Always else Never
        | 279 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 280 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 281 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 282 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 283 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 284 -> if PlayerData.instance.mapDeepnest then Always else Never
        | 285 -> if PlayerData.instance.mapDeepnest then Always else Never
        | 286 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 287 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 288 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 289 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 290 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 291 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 292 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 293 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 294 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 295 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 296 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 297 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 298 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 299 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 300 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 301 ->
            if PlayerData.instance.mapRoyalGardens then
                Visited
            else
                Never
        | 302 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 303 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 304 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 305 -> Passthru
        | 306 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 307 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 308 -> if PlayerData.instance.mapOutskirts then Always else Never
        | 309 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 310 -> if PlayerData.instance.mapOutskirts then Always else Never
        | 311 -> if PlayerData.instance.mapOutskirts then Always else Never
        | 312 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 313 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 314 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 315 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 316 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 317 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 318 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 319 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 320 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 321 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 322 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 323 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 324 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 326 -> Passthru
        | 327 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 328 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 329 -> if PlayerData.instance.mapAbyss then Always else Never
        | 330 -> if PlayerData.instance.mapDeepnest then Visited else Never
        | 331 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 332 -> if PlayerData.instance.mapAbyss then Always else Never
        | 333 -> if PlayerData.instance.mapAbyss then Always else Never
        | 334 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 335 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 336 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 337 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 338 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 339 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 340 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 341 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 342 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 343 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 344 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 345 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 346 -> if PlayerData.instance.mapAbyss then Visited else Never
        | 347 -> Passthru
        | 348 -> Passthru
        | 349 -> if PlayerData.instance.mapWaterways then Always else Never
        | 350 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 351 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 352 -> if PlayerData.instance.mapWaterways then Always else Never
        | 353 -> if PlayerData.instance.mapWaterways then Always else Never
        | 354 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 356 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 357 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 358 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 359 -> if PlayerData.instance.mapWaterways then Always else Never
        | 360 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 362 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 363 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 364 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 384 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 385 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 386 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 387 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 388 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 389 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 408 -> Passthru
        | 409 -> Passthru
        | 423 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 451 -> if PlayerData.instance.mapOutskirts then Visited else Never
        | 458 -> if PlayerData.instance.mapWaterways then Visited else Never
        | 471 -> Passthru
        | _ -> Never

    let doorTarget s d =
        match s, d with
        | 6, "right1" -> 7, "left1"
        | 6, "top1" -> 231, "bot2"
        | 6, "top2" -> 231, "bot1"
        | 7, "door_jiji" -> 28, "left1"
        | 7, "door_mapper" -> 12, "left1"
        | 7, "bot1" -> 37, "top1"
        | 7, "left1" -> 6, "right1"
        | 7, "right1" -> 255, "left1"
        | 7, "top1" -> 231, "right1"
        | 7, "room_divine" -> 390, "left1"
        | 9, "left1" -> 7, "door_station"
        | 10, "left1" -> 40, "door_charmshop"
        | 11, "left1" -> 40, "door_Mender_House"
        | 12, "left1" -> 7, "door_mapper"
        | 13, "left1" -> 231, "door1"
        | 14, "left1" -> 144, "door1"
        | 15, "left1" -> 310, "door1"
        | 16, "left1" -> 91, "door1"
        | 17, "left1" -> 7, "door_sly"
        | 18, "top1" -> 17, "door1"
        | 19, "left1" -> 38, "door1"
        | 20, "left1" -> 40, "door1"
        | 21, "right1" -> 300, "left1"
        | 22, "left1" -> 246, "door_Mansion"
        | 25, "right1" -> 7, "door_bretta"
        | 26, "top1" -> 25, "door1"
        | 27, "left1" -> 222, "door1"
        | 28, "left1" -> 7, "door_jiji"
        | 29, "left1" -> 7, "door_jiji"
        | 30, "left1" -> 313, "right1"
        | 30, "bot1" -> 31, "top1"
        | 31, "top1" -> 30, "bot1"
        | 31, "top2" -> 35, "bot1"
        | 32, "left1" -> 30, "right1"
        | 32, "left1 (1)" -> 30, "right1"
        | 33, "left1" -> 30, "right1"
        | 33, "left1 (1)" -> 30, "right1"
        | 34, "left1" -> 30, "right1"
        | 34, "left1 (1)" -> 30, "right1"
        | 35, "bot1" -> 31, "top2"
        | 36, "left1" -> 154, "door_SlugShrine"
        | 37, "left1" -> 43, "right1"
        | 37, "right1" -> 38, "left1"
        | 37, "top1" -> 7, "bot1"
        | 37, "top2" -> 7, "bot1"
        | 38, "left1" -> 37, "right1"
        | 38, "right1" -> 69, "left1"
        | 39, "left1" -> 58, "right1"
        | 39, "left2" -> 77, "right1"
        | 39, "right1" -> 54, "left1"
        | 39, "top1" -> 55, "bot1"
        | 39, "bot1" -> 57, "top1"
        | 39, "right2" -> 272, "left1"
        | 40, "left1" -> 57, "right1"
        | 40, "top1" -> 61, "bot1"
        | 40, "door1" -> 20, "left1"
        | 40, "right1" -> 81, "left1"
        | 41, "left1" -> 43, "right2"
        | 41, "right1" -> 70, "left1"
        | 42, "left1" -> 64, "right1"
        | 42, "right1" -> 46, "left1"
        | 43, "bot1" -> 64, "top1"
        | 43, "left1" -> 68, "right1"
        | 43, "left2" -> 50, "right1"
        | 43, "left3" -> 60, "right1"
        | 43, "right1" -> 37, "left1"
        | 43, "right2" -> 41, "left1"
        | 44, "left1" -> 64, "right2"
        | 44, "left2" -> 56, "right1"
        | 44, "right1" -> 62, "left1"
        | 44, "right2" -> 52, "left1"
        | 45, "left1" -> 66, "right2"
        | 45, "right1" -> 64, "left1"
        | 46, "left1" -> 42, "right1"
        | 46, "right1" -> 58, "left1"
        | 50, "right1" -> 43, "left2"
        | 50, "left1" -> 128, "right1"
        | 51, "left1" -> 65, "right1"
        | 51, "right1" -> 64, "left2"
        | 52, "left1" -> 44, "right2"
        | 52, "right1" -> 71, "left1"
        | 53, "right1" -> 78, "left1"
        | 53, "right2" -> 73, "left1"
        | 53, "left1" -> 69, "right1"
        | 53, "left2" -> 55, "right1"
        | 54, "left1" -> 39, "right1"
        | 54, "right1" -> 61, "left1"
        | 55, "bot1" -> 39, "top1"
        | 55, "left1" -> 70, "right1"
        | 55, "right1" -> 53, "left2"
        | 56, "right2" -> 82, "left1"
        | 56, "bot1" -> 170, "top1"
        | 56, "right1" -> 44, "left2"
        | 57, "left1" -> 71, "right1"
        | 57, "left2" -> 72, "right1"
        | 57, "right1" -> 40, "left1"
        | 57, "top1" -> 39, "bot1"
        | 58, "left1" -> 46, "right1"
        | 58, "right1" -> 39, "left1"
        | 58, "top1" -> 59, "bot1"
        | 58, "top2" -> 59, "bot2"
        | 59, "bot1" -> 58, "top1"
        | 60, "left1" -> 66, "right1"
        | 60, "right1" -> 43, "left3"
        | 61, "bot1" -> 40, "top1"
        | 61, "left1" -> 54, "right1"
        | 61, "left2" -> 63, "right1"
        | 61, "right1" -> 74, "left1"
        | 62, "left1" -> 44, "right1"
        | 63, "right1" -> 61, "left2"
        | 64, "left1" -> 45, "right1"
        | 64, "left2" -> 51, "right1"
        | 64, "right1" -> 42, "left1"
        | 64, "right2" -> 44, "left1"
        | 64, "top1" -> 43, "bot1"
        | 65, "right1" -> 51, "left1"
        | 65, "bot1" -> 213, "top1"
        | 66, "right1" -> 60, "left1"
        | 66, "right2" -> 45, "left1"
        | 67, "right1" -> 79, "left1"
        | 68, "right1" -> 43, "left1"
        | 69, "left1" -> 38, "right1"
        | 69, "right1" -> 53, "left1"
        | 70, "left1" -> 41, "right1"
        | 70, "right1" -> 55, "left1"
        | 71, "left1" -> 52, "right1"
        | 71, "right1" -> 57, "left1"
        | 72, "left1" -> 79, "right1"
        | 72, "right1" -> 57, "left2"
        | 73, "left1" -> 53, "right2"
        | 73, "right1" -> 248, "left1"
        | 74, "left1" -> 61, "right1"
        | 75, "right1" -> 237, "left1"
        | 76, "left1" -> 42, "door1"
        | 77, "right1" -> 39, "left2"
        | 78, "left1" -> 53, "right1"
        | 79, "left1" -> 67, "right1"
        | 79, "right1" -> 72, "left1"
        | 80, "right1" -> 106, "left1"
        | 81, "right1" -> 241, "left1"
        | 81, "left1" -> 40, "right1"
        | 82, "left1" -> 56, "right2"
        | 83, "left1" -> 116, "door_Ruin_House_01"
        | 84, "left1" -> 116, "door_Ruin_House_02"
        | 85, "left2" -> 357, "door1"
        | 85, "left1" -> 116, "door_Ruin_House_03"
        | 86, "left2" -> 87, "door1"
        | 86, "left1" -> 116, "door_Ruin_Elevator"
        | 87, "right1" -> 123, "left1"
        | 87, "door1" -> 86, "left2"
        | 88, "top1" -> 97, "bot1"
        | 88, "left1" -> 186, "right1"
        | 88, "bot1" -> 89, "top1"
        | 89, "top1" -> 88, "bot1"
        | 89, "bot1" -> 90, "top1"
        | 90, "left1" -> 91, "right1"
        | 90, "right2" -> 93, "left1"
        | 90, "right1" -> 92, "left2"
        | 90, "top1" -> 89, "bot1"
        | 91, "right1" -> 90, "left1"
        | 92, "bot1" -> 93, "top1"
        | 92, "top1" -> 108, "bot1"
        | 92, "right2" -> 98, "left1"
        | 92, "left2" -> 90, "right1"
        | 92, "right1" -> 96, "left1"
        | 93, "right1" -> 105, "left1"
        | 93, "bot1" -> 349, "top1"
        | 93, "left1" -> 90, "right2"
        | 93, "top1" -> 92, "bot1"
        | 95, "left1" -> 97, "right1"
        | 95, "right1" -> 108, "left1"
        | 96, "top1" -> 99, "bot1"
        | 96, "left1" -> 92, "right1"
        | 97, "top1" -> 106, "bot1"
        | 97, "right1" -> 95, "left1"
        | 97, "bot1" -> 88, "top1"
        | 98, "right1" -> 113, "left1"
        | 98, "left1" -> 92, "right2"
        | 98, "right2" -> 111, "left2"
        | 99, "top1" -> 100, "bot1"
        | 99, "right2" -> 104, "left3"
        | 99, "right1" -> 104, "left2"
        | 99, "bot1" -> 96, "top1"
        | 99, "left1" -> 108, "right1"
        | 100, "left2" -> 101, "right2"
        | 100, "left1" -> 101, "right1"
        | 100, "right1" -> 104, "left1"
        | 100, "bot1" -> 99, "top1"
        | 101, "left2" -> 110, "right2"
        | 101, "left1" -> 110, "right1"
        | 101, "right1" -> 100, "left1"
        | 101, "right2" -> 100, "left2"
        | 104, "left1" -> 100, "right1"
        | 104, "left3" -> 99, "right2"
        | 104, "left2" -> 99, "right1"
        | 105, "left1" -> 93, "right1"
        | 105, "right1" -> 111, "left1"
        | 106, "right1" -> 107, "left1"
        | 106, "left1" -> 80, "right1"
        | 106, "bot1" -> 97, "top1"
        | 107, "left1" -> 106, "right1"
        | 108, "bot1" -> 92, "top1"
        | 108, "right1" -> 99, "left1"
        | 108, "left1" -> 95, "right1"
        | 110, "right2" -> 101, "left2"
        | 110, "right1" -> 101, "left1"
        | 111, "left2" -> 98, "right2"
        | 111, "top1" -> 113, "bot1"
        | 111, "right1" -> 116, "left1"
        | 111, "left1" -> 105, "right1"
        | 113, "top1" -> 127, "bot1"
        | 113, "left1" -> 98, "right1"
        | 113, "bot1" -> 111, "top1"
        | 116, "left2" -> 327, "right1"
        | 116, "door_Ruin_Elevator" -> 86, "left1"
        | 116, "door_Ruin_House_03" -> 85, "left1"
        | 116, "door_Ruin_House_01" -> 83, "left1"
        | 116, "door_Ruin_House_02" -> 84, "left1"
        | 116, "right2" -> 118, "left2"
        | 116, "right1" -> 118, "left1"
        | 116, "left1" -> 111, "right1"
        | 117, "top1" -> 121, "bot1"
        | 117, "bot1" -> 118, "top1"
        | 117, "left1" -> 123, "right2"
        | 118, "right2" -> 119, "left1"
        | 118, "left2" -> 116, "right2"
        | 118, "right1" -> 120, "left1"
        | 118, "left1" -> 116, "right1"
        | 118, "top1" -> 117, "bot1"
        | 119, "right1" -> 308, "left1"
        | 119, "top1" -> 124, "bot1"
        | 119, "left1" -> 118, "right2"
        | 120, "left1" -> 118, "right1"
        | 121, "bot1" -> 117, "top1"
        | 122, "left1" -> 241, "right1"
        | 122, "right1" -> 245, "left1"
        | 123, "left1" -> 87, "right1"
        | 123, "right1" -> 313, "left1"
        | 123, "right2" -> 117, "left1"
        | 124, "bot1" -> 119, "top1"
        | 124, "right1" -> 311, "left2"
        | 127, "bot1" -> 113, "top1"
        | 128, "left1" -> 129, "right1"
        | 128, "right1" -> 50, "left1"
        | 129, "right1" -> 128, "left1"
        | 129, "left1" -> 130, "right1"
        | 130, "left1" -> 146, "right1"
        | 130, "right1" -> 129, "left1"
        | 130, "right2" -> 135, "left1"
        | 131, "bot1" -> 134, "top1"
        | 131, "right1" -> 146, "left1"
        | 131, "left1" -> 158, "right1"
        | 132, "right1" -> 149, "left1"
        | 132, "left1" -> 153, "right1"
        | 134, "right1" -> 143, "left1"
        | 134, "bot1" -> 139, "top1"
        | 134, "top1" -> 131, "bot1"
        | 135, "bot1" -> 136, "top1"
        | 135, "left1" -> 130, "right2"
        | 136, "right1" -> 137, "left1"
        | 136, "top1" -> 135, "bot1"
        | 136, "left1" -> 147, "right1"
        | 137, "left1" -> 136, "right1"
        | 138, "left1" -> 144, "right1"
        | 138, "right1" -> 157, "left1"
        | 139, "right1" -> 147, "left1"
        | 139, "left1" -> 157, "right1"
        | 139, "top1" -> 134, "bot1"
        | 140, "right1" -> 160, "left1"
        | 140, "bot1" -> 197, "top1"
        | 140, "top1" -> 147, "bot1"
        | 140, "left1" -> 156, "right1"
        | 140, "right2" -> 163, "left1"
        | 141, "right1" -> 156, "left1"
        | 141, "left1" -> 142, "right1"
        | 142, "right1" -> 141, "left1"
        | 142, "left1" -> 207, "right1"
        | 143, "left1" -> 134, "right1"
        | 144, "right1" -> 138, "left1"
        | 144, "door1" -> 14, "left1"
        | 145, "right1" -> 150, "left1"
        | 146, "right1" -> 130, "left1"
        | 146, "left1" -> 131, "right1"
        | 147, "left1" -> 139, "right1"
        | 147, "right1" -> 136, "left1"
        | 147, "bot1" -> 140, "top1"
        | 148, "bot1" -> 149, "top1"
        | 148, "bot2" -> 159, "top1"
        | 148, "right1" -> 155, "left2"
        | 149, "top1" -> 148, "bot1"
        | 149, "right1" -> 159, "left1"
        | 149, "bot1" -> 150, "top1"
        | 149, "left1" -> 132, "right1"
        | 150, "left1" -> 145, "right1"
        | 150, "bot1" -> 157, "top1"
        | 150, "top1" -> 149, "bot1"
        | 151, "left1" -> 224, "right2"
        | 151, "right1" -> 205, "left1"
        | 152, "left1" -> 201, "right1"
        | 153, "right1" -> 132, "left1"
        | 153, "left1" -> 154, "right1"
        | 154, "left1" -> 164, "right1"
        | 154, "door_SlugShrine" -> 36, "left1"
        | 154, "right1" -> 153, "left1"
        | 155, "left2" -> 148, "right1"
        | 155, "left1" -> 230, "right3"
        | 156, "right1" -> 140, "left1"
        | 156, "left1" -> 141, "right1"
        | 157, "top3" -> 158, "bot1"
        | 157, "top1" -> 150, "bot1"
        | 157, "right1" -> 139, "left1"
        | 157, "left1" -> 138, "right1"
        | 158, "right1" -> 131, "left1"
        | 158, "bot1" -> 157, "top3"
        | 158, "top1" -> 159, "bot1"
        | 159, "left1" -> 149, "right1"
        | 159, "top1" -> 148, "bot2"
        | 159, "bot1" -> 158, "top1"
        | 160, "door1" -> 161, "left1"
        | 160, "left1" -> 140, "right1"
        | 161, "right1" -> 162, "left1"
        | 161, "left1" -> 160, "door1"
        | 162, "left1" -> 161, "right1"
        | 163, "left1" -> 140, "right2"
        | 164, "right1" -> 154, "left1"
        | 165, "right1" -> 167, "left1"
        | 165, "left1" -> 198, "right2"
        | 165, "left2" -> 166, "right1"
        | 165, "left3" -> 196, "right1"
        | 166, "right1" -> 165, "left2"
        | 167, "right1" -> 168, "left1"
        | 167, "left1" -> 165, "right1"
        | 167, "bot1" -> 183, "top1"
        | 168, "right2" -> 190, "left2"
        | 168, "right1" -> 190, "left1"
        | 168, "top1" -> 169, "bot1"
        | 168, "left1" -> 167, "right1"
        | 169, "bot1" -> 168, "top1"
        | 169, "right1" -> 170, "left1"
        | 170, "left2" -> 195, "right1"
        | 170, "right2" -> 171, "left1"
        | 170, "left1" -> 169, "right1"
        | 170, "top1" -> 56, "bot1"
        | 170, "right1" -> 189, "left1"
        | 171, "left1" -> 170, "right2"
        | 171, "right1" -> 172, "left1"
        | 172, "right1" -> 194, "left1"
        | 172, "left1" -> 171, "right1"
        | 172, "left2" -> 173, "right1"
        | 173, "left1" -> 174, "right1"
        | 173, "right1" -> 172, "left2"
        | 174, "right1" -> 173, "left1"
        | 174, "right2" -> 186, "left1"
        | 174, "bot1" -> 175, "top1"
        | 175, "top1" -> 174, "bot1"
        | 175, "left1" -> 183, "right1"
        | 175, "left2" -> 182, "right1"
        | 175, "right1" -> 176, "left1"
        | 176, "bot1" -> 177, "top1"
        | 176, "left1" -> 175, "right1"
        | 177, "top1" -> 176, "bot1"
        | 177, "left3" -> 187, "right1"
        | 177, "left2" -> 178, "right1"
        | 178, "bot3" -> 179, "top2"
        | 178, "bot2" -> 179, "top2"
        | 178, "top1" -> 182, "bot1"
        | 178, "right1" -> 177, "left2"
        | 178, "bot1" -> 179, "top2"
        | 179, "right1" -> 193, "left1"
        | 179, "left1" -> 188, "right1"
        | 179, "top1" -> 178, "bot1"
        | 179, "top2" -> 178, "bot3"
        | 179, "top3" -> 178, "bot3"
        | 182, "left1" -> 191, "right1"
        | 182, "bot1" -> 178, "top1"
        | 182, "right1" -> 175, "left2"
        | 183, "bot1" -> 184, "top1"
        | 183, "right1" -> 175, "left1"
        | 183, "top1" -> 167, "bot1"
        | 184, "top1" -> 183, "bot1"
        | 184, "left1" -> 185, "right1"
        | 185, "left1" -> 277, "right1"
        | 185, "right1" -> 184, "left1"
        | 186, "left1" -> 174, "right2"
        | 186, "right1" -> 88, "left1"
        | 187, "right2" -> 359, "left1"
        | 187, "right1" -> 177, "left3"
        | 188, "top1" -> 284, "bot1"
        | 188, "top2" -> 192, "bot1"
        | 188, "right1" -> 179, "left1"
        | 188, "right1 (1)" -> 179, "left1"
        | 189, "left1" -> 170, "right1"
        | 190, "left2" -> 168, "right2"
        | 190, "left1" -> 168, "right1"
        | 191, "bot1" -> 192, "top1"
        | 191, "right1" -> 182, "left1"
        | 192, "bot1" -> 188, "top2"
        | 192, "top1" -> 191, "bot1"
        | 193, "left1" -> 179, "right1"
        | 194, "left1" -> 172, "right1"
        | 195, "right1" -> 170, "left2"
        | 195, "left1" -> 213, "right1"
        | 196, "right1" -> 165, "left3"
        | 197, "right2" -> 198, "left1"
        | 197, "left1" -> 210, "right1"
        | 197, "top1" -> 140, "bot1"
        | 197, "right1" -> 211, "left1"
        | 198, "right1" -> 223, "left1"
        | 198, "left3" -> 218, "right1"
        | 198, "left1" -> 197, "right2"
        | 198, "right2" -> 165, "left1"
        | 198, "left2" -> 199, "right1"
        | 199, "left1" -> 217, "right1"
        | 199, "right1" -> 198, "left2"
        | 200, "left1" -> 206, "right1"
        | 200, "right2" -> 201, "left1"
        | 200, "right1" -> 217, "left1"
        | 200, "left2" -> 205, "right1"
        | 201, "right2" -> 204, "left1"
        | 201, "left1" -> 200, "right2"
        | 201, "right1" -> 152, "left1"
        | 202, "left1" -> 301, "right1"
        | 202, "top1" -> 203, "bot1"
        | 202, "right1" -> 204, "left2"
        | 203, "bot1" -> 202, "top1"
        | 203, "top1" -> 205, "bot1"
        | 204, "right1" -> 219, "left1"
        | 204, "left1" -> 201, "right2"
        | 204, "left2" -> 202, "right1"
        | 205, "left3" -> 225, "right1"
        | 205, "bot1" -> 203, "top1"
        | 205, "left2" -> 220, "right1"
        | 205, "left1" -> 151, "right1"
        | 205, "right1" -> 200, "left2"
        | 206, "top1" -> 207, "bot1"
        | 206, "right1" -> 200, "left1"
        | 207, "right1" -> 142, "left1"
        | 207, "left1" -> 208, "right1"
        | 207, "bot1" -> 206, "top1"
        | 208, "left1" -> 224, "right1"
        | 208, "right1" -> 207, "left1"
        | 210, "left1" -> 222, "right1"
        | 210, "top1" -> 216, "bot1"
        | 210, "right1" -> 197, "left1"
        | 211, "right1" -> 212, "left1"
        | 211, "left1" -> 197, "right1"
        | 212, "left1" -> 211, "right1"
        | 212, "right1" -> 213, "left2"
        | 213, "right1" -> 195, "left1"
        | 213, "left1" -> 215, "right1"
        | 213, "top1" -> 65, "bot1"
        | 213, "left3" -> 214, "right1"
        | 213, "left2" -> 212, "right1"
        | 214, "right1" -> 213, "left3"
        | 214, "left1" -> 223, "right1"
        | 215, "right1" -> 213, "left1"
        | 216, "bot1" -> 210, "top1"
        | 217, "right1" -> 199, "left1"
        | 217, "left1" -> 200, "right1"
        | 217, "top1" -> 222, "bot1"
        | 218, "right1" -> 198, "left3"
        | 219, "right1" -> 277, "left1"
        | 219, "left1" -> 204, "right1"
        | 220, "top1" -> 224, "bot1"
        | 220, "right1" -> 205, "left2"
        | 222, "bot1" -> 217, "top1"
        | 222, "right1" -> 210, "left1"
        | 223, "door1" -> 227, "left1"
        | 223, "right1" -> 214, "left1"
        | 223, "left1" -> 198, "right1"
        | 224, "bot1" -> 220, "top1"
        | 224, "right1" -> 208, "left1"
        | 224, "right2" -> 151, "left1"
        | 225, "right1" -> 205, "left3"
        | 226, "right1" -> 301, "left1"
        | 227, "bot1" -> 228, "top1"
        | 227, "left1" -> 223, "door1"
        | 228, "top1" -> 227, "bot1"
        | 230, "right2" -> 234, "left1"
        | 230, "right3" -> 155, "left1"
        | 230, "right1" -> 231, "left1"
        | 230, "right4" -> 236, "left1"
        | 231, "right1" -> 7, "top1"
        | 231, "bot2" -> 6, "top1"
        | 231, "left1" -> 230, "right1"
        | 231, "bot1" -> 6, "top2"
        | 231, "left2" -> 233, "right1"
        | 231, "door1" -> 13, "left1"
        | 233, "right1" -> 231, "left2"
        | 234, "right1" -> 235, "left1"
        | 234, "left1" -> 230, "right2"
        | 235, "left1" -> 234, "right1"
        | 236, "left1" -> 230, "right4"
        | 237, "bot1" -> 241, "top1"
        | 237, "left1" -> 75, "right1"
        | 237, "right1" -> 239, "left1"
        | 237, "top1" -> 267, "bot1"
        | 239, "right1" -> 240, "left1"
        | 239, "left1" -> 237, "right1"
        | 240, "left2" -> 242, "right1"
        | 240, "right2" -> 244, "left1"
        | 240, "right1" -> 243, "left1"
        | 240, "left1" -> 239, "right1"
        | 240, "bot1" -> 245, "top1"
        | 240, "left3" -> 247, "right1"
        | 241, "left1" -> 81, "right1"
        | 241, "right1" -> 122, "left1"
        | 241, "top1" -> 237, "bot1"
        | 242, "right1" -> 240, "left2"
        | 243, "left1" -> 240, "right1"
        | 244, "left1" -> 240, "right2"
        | 245, "top2" -> 246, "bot1"
        | 245, "top1" -> 240, "bot1"
        | 245, "left1" -> 122, "right1"
        | 246, "bot1" -> 245, "top2"
        | 246, "door_Mansion" -> 22, "left1"
        | 247, "right1" -> 240, "left3"
        | 248, "left1" -> 73, "right1"
        | 248, "bot1" -> 249, "top1"
        | 249, "left1" -> 272, "right1"
        | 249, "right1" -> 268, "left1"
        | 249, "top2" -> 250, "bot1"
        | 249, "top1" -> 248, "bot1"
        | 250, "bot1" -> 249, "top2"
        | 250, "top1" -> 252, "bot1"
        | 250, "right1" -> 259, "left1"
        | 251, "right1" -> 254, "left1"
        | 251, "top1" -> 276, "bot1"
        | 251, "left3" -> 268, "right2"
        | 251, "left1" -> 259, "right1"
        | 251, "left2" -> 268, "right1"
        | 252, "right1" -> 262, "left1"
        | 252, "top1" -> 256, "bot1"
        | 252, "left1" -> 269, "right1"
        | 252, "bot1" -> 250, "top1"
        | 252, "left2" -> 253, "right1"
        | 253, "left1" -> 275, "right1"
        | 253, "right1" -> 252, "left2"
        | 254, "left1" -> 251, "right1"
        | 254, "right1" -> 267, "left1"
        | 255, "right1" -> 269, "left1"
        | 255, "left1" -> 7, "right1"
        | 255, "bot1" -> 258, "top1"
        | 256, "bot1" -> 252, "top1"
        | 256, "right1" -> 260, "left1"
        | 256, "top1" -> 257, "bot1"
        | 257, "top1" -> 273, "left1"
        | 257, "right1" -> 263, "left1"
        | 257, "bot1" -> 256, "top1"
        | 258, "top1" -> 255, "bot1"
        | 259, "right1" -> 251, "left1"
        | 259, "left1" -> 250, "right1"
        | 260, "right1" -> 263, "left2"
        | 260, "left1" -> 256, "right1"
        | 260, "top1" -> 271, "bot1"
        | 262, "right1" -> 263, "left3"
        | 262, "left1" -> 252, "right1"
        | 263, "bot1" -> 276, "top1"
        | 263, "right2" -> 270, "left1"
        | 263, "right1" -> 264, "left1"
        | 263, "left2" -> 260, "right1"
        | 263, "left1" -> 257, "right1"
        | 263, "left3" -> 262, "right1"
        | 264, "top1" -> 273, "bot2"
        | 264, "left1" -> 263, "right1"
        | 264, "right1" -> 266, "left1"
        | 264, "right2" -> 265, "left1"
        | 265, "left1" -> 264, "right2"
        | 266, "top1" -> 273, "bot1"
        | 266, "left1" -> 264, "right1"
        | 267, "door1" -> 274, "left1"
        | 267, "left1" -> 254, "right1"
        | 267, "bot1" -> 237, "top1"
        | 268, "right1" -> 251, "left2"
        | 268, "right2" -> 251, "left3"
        | 268, "left1" -> 249, "right1"
        | 269, "right1" -> 252, "left1"
        | 269, "left1" -> 255, "right1"
        | 270, "left1" -> 263, "right2"
        | 271, "bot1" -> 260, "top1"
        | 272, "right1" -> 249, "left1"
        | 272, "left1" -> 39, "right2"
        | 273, "right1" -> 257, "top1"
        | 273, "left1" -> 257, "top1"
        | 273, "bot1" -> 266, "top1"
        | 273, "bot2" -> 264, "top1"
        | 274, "left1" -> 267, "door1"
        | 275, "right1" -> 253, "left1"
        | 276, "top1" -> 263, "bot1"
        | 276, "bot1" -> 251, "top1"
        | 277, "right1" -> 185, "left1"
        | 277, "left1" -> 219, "right1"
        | 277, "bot2" -> 278, "top2"
        | 277, "bot1" -> 278, "top1"
        | 278, "right1" -> 279, "left1"
        | 278, "right2" -> 279, "left2"
        | 278, "bot1" -> 285, "top1"
        | 278, "top2" -> 277, "bot2"
        | 278, "top1" -> 277, "bot1"
        | 279, "right1" -> 294, "left1"
        | 279, "left1" -> 278, "right1"
        | 279, "left2" -> 278, "right2"
        | 280, "left1" -> 292, "right1"
        | 280, "left2" -> 289, "right1"
        | 280, "right1" -> 288, "left1"
        | 280, "top1" -> 291, "bot1"
        | 281, "left1" -> 282, "right1"
        | 282, "door2" -> 305, "left1"
        | 282, "right1" -> 281, "left1"
        | 282, "right2" -> 299, "left1"
        | 282, "right3" -> 299, "left2"
        | 283, "bot1" -> 291, "top1"
        | 283, "bot2" -> 291, "top2"
        | 283, "left1" -> 286, "right1"
        | 283, "right1" -> 285, "left1"
        | 283, "left1 (1)" -> 286, "right1"
        | 283, "left1 (2)" -> 286, "right1"
        | 283, "left1 (3)" -> 286, "right1"
        | 284, "bot1" -> 188, "top1"
        | 284, "left1" -> 285, "right1"
        | 285, "bot1" -> 288, "top1"
        | 285, "left1" -> 283, "right1"
        | 285, "right1" -> 284, "left1"
        | 285, "top1" -> 278, "bot1"
        | 286, "bot1" -> 293, "top1"
        | 286, "right1" -> 283, "left1"
        | 286, "right1 (1)" -> 283, "left1"
        | 286, "right1 (2)" -> 283, "left1"
        | 286, "right1 (3)" -> 283, "left1"
        | 288, "left1" -> 280, "right1"
        | 288, "top1" -> 285, "bot1"
        | 288, "right1" -> 295, "left1"
        | 289, "right2" -> 290, "left1"
        | 289, "right1" -> 280, "left2"
        | 290, "left1" -> 289, "right2"
        | 291, "bot1" -> 280, "top1"
        | 291, "top1" -> 283, "bot1"
        | 291, "top2" -> 283, "bot2"
        | 292, "left1" -> 297, "right1"
        | 292, "top1" -> 293, "bot1"
        | 292, "right1" -> 280, "left1"
        | 293, "left1" -> 298, "right1"
        | 293, "bot1" -> 292, "top1"
        | 293, "top1" -> 286, "bot1"
        | 294, "left1" -> 279, "right1"
        | 295, "bot1" -> 302, "top1"
        | 295, "top1" -> 296, "bot1"
        | 295, "left1" -> 288, "right1"
        | 295, "right1" -> 330, "left1"
        | 296, "bot1" -> 295, "top1"
        | 297, "door1" -> 303, "left1"
        | 297, "right1" -> 292, "left1"
        | 297, "top1" -> 300, "bot1"
        | 297, "left1" -> 299, "right1"
        | 298, "right1" -> 293, "left1"
        | 299, "left2" -> 282, "right3"
        | 299, "right1" -> 297, "left1"
        | 299, "left1" -> 282, "right2"
        | 300, "top1" -> 301, "bot1"
        | 300, "bot1" -> 297, "top1"
        | 300, "left1" -> 21, "right1"
        | 301, "left1" -> 226, "right1"
        | 301, "bot1" -> 300, "top1"
        | 301, "right1" -> 202, "left1"
        | 302, "top1" -> 295, "bot1"
        | 303, "left1" -> 297, "door1"
        | 304, "left1" -> 282, "door1"
        | 305, "left1" -> 282, "door2"
        | 306, "top1" -> 307, "bot1"
        | 306, "bot1" -> 331, "top1"
        | 306, "right1" -> 386, "left1"
        | 307, "bot1" -> 306, "top1"
        | 307, "bot2" -> 386, "top1"
        | 307, "right1" -> 308, "left2"
        | 307, "top1" -> 363, "bot2"
        | 308, "top1" -> 311, "bot1"
        | 308, "right1" -> 309, "left1"
        | 308, "right2" -> 310, "left1"
        | 308, "left2" -> 307, "right1"
        | 308, "left1" -> 119, "right1"
        | 309, "right2" -> 320, "left1"
        | 309, "left2" -> 311, "right1"
        | 309, "right1" -> 315, "left1"
        | 309, "left1" -> 308, "right1"
        | 310, "right1" -> 321, "left1"
        | 310, "bot1" -> 318, "top1"
        | 310, "top1" -> 323, "bot1"
        | 310, "door1" -> 15, "left1"
        | 310, "left1" -> 308, "right2"
        | 311, "left2" -> 124, "right1"
        | 311, "left1" -> 312, "right1"
        | 311, "right1" -> 309, "left2"
        | 311, "bot2" -> 308, "top2"
        | 311, "bot1" -> 308, "top1"
        | 312, "top1" -> 313, "bot1"
        | 312, "right1" -> 311, "left1"
        | 313, "right1" -> 30, "left1"
        | 313, "left1" -> 123, "right1"
        | 313, "bot1" -> 312, "top1"
        | 314, "left1" -> 323, "right2"
        | 315, "bot1" -> 323, "top1"
        | 315, "top1" -> 317, "bot1"
        | 315, "left1" -> 309, "right1"
        | 315, "right1" -> 316, "left1"
        | 316, "right1" -> 324, "left1"
        | 316, "left1" -> 315, "right1"
        | 317, "bot1" -> 315, "top1"
        | 318, "door1" -> 322, "left1"
        | 318, "top1" -> 310, "bot1"
        | 318, "top2" -> 321, "bot1"
        | 320, "left1" -> 309, "right2"
        | 321, "bot1" -> 318, "top2"
        | 321, "left1" -> 310, "right1"
        | 322, "left1" -> 318, "door1"
        | 323, "top1" -> 315, "bot1"
        | 323, "right2" -> 314, "left1"
        | 323, "bot1" -> 310, "top1"
        | 324, "left2" -> 326, "right1"
        | 324, "left1" -> 316, "right1"
        | 326, "right1" -> 324, "left2"
        | 327, "left1" -> 354, "right1"
        | 327, "right1" -> 116, "left2"
        | 327, "left3" -> 328, "right1"
        | 327, "left2" -> 356, "right1"
        | 327, "right2" -> 357, "left1"
        | 328, "bot1" -> 329, "top1"
        | 328, "right1" -> 327, "left3"
        | 329, "top1" -> 328, "bot1"
        | 329, "bot1" -> 341, "top1"
        | 329, "bot2" -> 332, "top1"
        | 330, "left1" -> 295, "right1"
        | 331, "top1" -> 306, "bot1"
        | 331, "right1" -> 384, "left1"
        | 332, "bot1" -> 334, "top1"
        | 332, "top1" -> 329, "bot2"
        | 332, "right1" -> 333, "left1"
        | 332, "left1" -> 342, "right1"
        | 333, "right1" -> 346, "left1"
        | 333, "left1" -> 332, "right1"
        | 334, "bot1" -> 339, "top1"
        | 334, "left1" -> 335, "right1"
        | 334, "left1 extra" -> 335, "right1"
        | 334, "left3" -> 338, "right1"
        | 334, "right2" -> 340, "left1"
        | 334, "top1" -> 332, "bot1"
        | 335, "right1" -> 334, "left1"
        | 336, "right3" -> 337, "left2"
        | 336, "right1" -> 337, "left1"
        | 336, "left1" -> 340, "right1"
        | 336, "right2" -> 347, "left1"
        | 337, "left2" -> 336, "right3"
        | 337, "left1" -> 336, "right1"
        | 338, "right1" -> 334, "left3"
        | 339, "top1" -> 334, "bot1"
        | 340, "right1" -> 336, "left1"
        | 340, "left1" -> 334, "right2"
        | 341, "top1" -> 329, "bot1"
        | 342, "right1" -> 332, "left1"
        | 342, "left1" -> 343, "right1"
        | 343, "right1" -> 342, "left1"
        | 343, "left1" -> 345, "right1"
        | 343, "bot1" -> 344, "top1"
        | 343, "bot2" -> 344, "top2"
        | 344, "top1" -> 343, "bot1"
        | 344, "top2" -> 343, "bot2"
        | 345, "right1" -> 343, "left1"
        | 346, "left1" -> 333, "right1"
        | 347, "left1" -> 336, "right2"
        | 348, "left1" -> 224, "door1"
        | 349, "top1" -> 93, "bot1"
        | 349, "right1" -> 351, "left1"
        | 349, "bot1" -> 350, "top1"
        | 349, "left1" -> 352, "right1"
        | 350, "top3" -> 352, "bot1"
        | 350, "bot1" -> 358, "top1"
        | 350, "top2" -> 354, "bot1"
        | 350, "top1" -> 349, "bot1"
        | 350, "bot2" -> 356, "top1"
        | 351, "left1" -> 349, "right1"
        | 352, "bot1" -> 350, "top3"
        | 352, "left2" -> 353, "right2"
        | 352, "left1" -> 353, "right1"
        | 352, "right1" -> 349, "left1"
        | 353, "right2" -> 352, "left2"
        | 353, "left1" -> 359, "right1"
        | 353, "right1" -> 352, "left1"
        | 354, "bot2" -> 364, "top1"
        | 354, "right1" -> 327, "left1"
        | 354, "bot1" -> 350, "top2"
        | 356, "right1" -> 327, "left2"
        | 356, "top1" -> 350, "bot2"
        | 357, "top1" -> 363, "bot1"
        | 357, "right1" -> 362, "left1"
        | 357, "left1" -> 327, "right2"
        | 357, "door1" -> 85, "left2"
        | 357, "right2" -> 362, "left2"
        | 358, "left1" -> 360, "right1"
        | 358, "top1" -> 350, "bot1"
        | 359, "right1" -> 353, "left1"
        | 359, "left1" -> 187, "right2"
        | 360, "right1" -> 358, "left1"
        | 362, "left2" -> 357, "right2"
        | 362, "left1" -> 357, "right1"
        | 363, "bot1" -> 357, "top1"
        | 363, "bot2" -> 307, "top1"
        | 364, "top1" -> 354, "bot2"
        | 365, "right1" -> 366, "left1"
        | 365, "left1" -> 374, "door2"
        | 365, "top1" -> 367, "bot1"
        | 366, "left1" -> 365, "right1"
        | 367, "top1" -> 370, "bot1"
        | 367, "bot1" -> 365, "top1"
        | 367, "right1" -> 378, "left1"
        | 367, "left1" -> 377, "right1"
        | 367, "left2" -> 368, "right2"
        | 368, "top1" -> 377, "bot1"
        | 368, "right2" -> 367, "left2"
        | 369, "left1" -> 378, "right1"
        | 369, "right1" -> 379, "left1"
        | 369, "right2" -> 379, "left2"
        | 369, "left2" -> 378, "right2"
        | 370, "left1" -> 381, "right1"
        | 370, "bot1" -> 367, "top1"
        | 370, "top1" -> 371, "bot1"
        | 371, "top1" -> 375, "bot1"
        | 371, "bot1" -> 370, "top1"
        | 372, "right1" -> 376, "left3"
        | 372, "left1" -> 376, "right1"
        | 373, "right1" -> 376, "left1"
        | 374, "door2" -> 365, "left1"
        | 375, "right1" -> 376, "left2"
        | 375, "bot1" -> 371, "top1"
        | 376, "right1" -> 372, "left1"
        | 376, "left3" -> 372, "right1"
        | 376, "left1" -> 373, "right1"
        | 376, "left2" -> 375, "right1"
        | 377, "bot1" -> 368, "top1"
        | 377, "right1" -> 367, "left1"
        | 378, "right1" -> 369, "left1"
        | 378, "left1" -> 367, "right1"
        | 378, "right2" -> 369, "left2"
        | 379, "left1" -> 369, "right1"
        | 379, "left2" -> 369, "right2"
        | 380, "right1" -> 382, "left1"
        | 380, "bot1" -> 381, "top1"
        | 381, "top1" -> 380, "bot1"
        | 381, "right1" -> 370, "left1"
        | 382, "top1" -> 383, "bot1"
        | 382, "left1" -> 380, "right1"
        | 383, "bot1" -> 382, "top1"
        | 384, "right2" -> 385, "left3"
        | 384, "left1" -> 331, "right1"
        | 384, "right1" -> 385, "left2"
        | 385, "left2" -> 384, "right1"
        | 385, "left3" -> 384, "right2"
        | 385, "left1" -> 386, "right3"
        | 386, "right3" -> 385, "left1"
        | 386, "right2" -> 388, "left2"
        | 386, "right1" -> 388, "left1"
        | 386, "left1" -> 306, "right1"
        | 386, "top1" -> 307, "bot2"
        | 388, "left2" -> 386, "right2"
        | 388, "right1" -> 389, "left1"
        | 388, "left1" -> 386, "right1"
        | 389, "left1" -> 388, "right1"
        | 390, "left1" -> 7, "room_divine"
        | 391, "left1" -> 7, "room_grimm"
        | 405, "left1" -> 404, "door2"
        | 408, "right1" -> 409, "left1"
        | 408, "left1" -> 19, "door1"
        | _ -> 0, ""

    let healthAnims =
        [| "Blue Appear",
           [| 1659, 437, 27, 31
              1625, 437, 34, 35
              1403, 437, 45, 43
              1448, 437, 51, 42
              1348, 437, 55, 45
              1760, 279, 89, 89
              978, 130, 143, 146
              1121, 130, 143, 146
              1264, 130, 143, 146 |]
           "Health Empty", [| 1281, 437, 67, 59 |]
           "Health Appear",
           [| 724, 0, 119, 130
              843, 0, 119, 130
              962, 0, 119, 130
              1081, 0, 119, 130
              1200, 0, 119, 130 |]
           "Health Bound", [| 761, 437, 65, 70 |]
           "Blue Break",
           [| 0, 0, 127, 124
              127, 0, 127, 124
              0, 130, 118, 116
              118, 130, 126, 114
              1499, 437, 126, 37 |]
           "Health Break",
           [| 1849, 279, 127, 158
              0, 437, 127, 158
              127, 437, 127, 158
              254, 437, 127, 158
              381, 437, 127, 158
              508, 437, 127, 158 |]
           "Health Idle",
           [| 826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              826, 437, 65, 70
              891, 437, 65, 70
              956, 437, 65, 70
              1021, 437, 65, 70
              1086, 437, 65, 70
              1151, 437, 65, 70
              1216, 437, 65, 70 |]
           "Blue Idle",
           [| 1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1506, 130, 99, 149
              1605, 130, 99, 149
              1704, 130, 99, 149
              1803, 130, 99, 149
              1902, 130, 99, 149
              0, 279, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              1407, 130, 99, 149
              99, 279, 99, 149
              198, 279, 99, 149
              297, 279, 99, 149
              396, 279, 99, 149
              495, 279, 99, 149
              594, 279, 99, 149
              693, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              792, 279, 99, 149
              891, 279, 99, 149
              990, 279, 99, 149
              1089, 279, 99, 149
              1188, 279, 99, 149
              1287, 279, 99, 149 |]
           "Blue Break Fast",
           [| 0, 0, 127, 124
              127, 0, 127, 124
              0, 130, 118, 116
              118, 130, 126, 114
              1499, 437, 126, 37 |]
           "Health Refill",
           [| 1319, 0, 119, 130
              1438, 0, 119, 130
              1557, 0, 119, 130
              1676, 0, 119, 130
              1795, 0, 119, 130
              1914, 0, 119, 130 |]
           "Health Max Up",
           [| 506, 0, 109, 117
              734, 130, 118, 142
              1638, 279, 122, 157
              615, 0, 109, 117
              365, 130, 123, 142
              635, 437, 126, 158
              254, 0, 126, 127
              488, 130, 123, 142
              1386, 279, 126, 157
              380, 0, 126, 127
              611, 130, 123, 142
              1512, 279, 126, 157
              852, 130, 126, 143
              244, 130, 121, 134 |] |]

    let grimmchildAnims =
        [| "Idle 3",
           [| 1131, 686, 158, 123
              1519, 0, 163, 177
              1130, 198, 133, 170
              1010, 374, 147, 157
              256, 686, 153, 132
              858, 537, 157, 139
              1855, 537, 158, 134 |]
           "Antic 3",
           [| 1419, 198, 155, 166
              684, 0, 115, 194
              312, 374, 135, 163
              1911, 374, 107, 151 |]
           "Fly 3",
           [| 94, 686, 162, 133
              536, 374, 141, 159
              1295, 537, 142, 135
              1447, 686, 152, 111
              1289, 686, 158, 123
              807, 686, 171, 125
              1746, 374, 165, 152 |]
           "Shoot 3", [| 0, 374, 159, 163; 1263, 198, 156, 169; 495, 198, 155, 172 |]
           "Sleep 3", [| 978, 686, 153, 124; 1741, 686, 132, 102; 0, 820, 109, 84 |]
           "Tele In 3",
           [| 795, 537, 63, 140
              0, 686, 94, 134
              608, 537, 98, 142
              1172, 537, 123, 138
              706, 537, 89, 141
              608, 686, 136, 131
              159, 374, 153, 163 |]
           "Tele Out 3",
           [| 706, 537, 89, 141
              1172, 537, 123, 138
              608, 537, 98, 142
              0, 686, 94, 134
              795, 537, 63, 140 |]
           "TurnToIdle 3",
           [| 452, 537, 156, 143
              291, 537, 161, 147
              1519, 0, 163, 177
              1130, 198, 133, 170
              1010, 374, 147, 157
              256, 686, 153, 132
              1015, 537, 157, 139
              1855, 537, 158, 134 |]
           "Wake 3", [| 1741, 686, 132, 102; 978, 686, 153, 124 |]
           "Antic 4", [| 1682, 0, 204, 176; 368, 0, 161, 196; 799, 0, 173, 190; 1157, 374, 145, 157 |]
           "Fly 4",
           [| 1359, 0, 160, 178
              1437, 537, 209, 134
              1574, 198, 156, 164
              677, 374, 148, 158
              103, 537, 188, 148
              1173, 0, 186, 178 |]
           "Idle 4",
           [| 1646, 537, 209, 134
              529, 0, 155, 194
              1543, 374, 203, 152
              341, 198, 154, 173
              650, 198, 138, 172
              825, 374, 185, 157
              0, 0, 184, 198 |]
           "Shoot 4", [| 0, 198, 181, 176; 926, 198, 204, 171 |]
           "Sleep 4", [| 181, 198, 160, 175; 1599, 686, 142, 105; 1873, 686, 133, 97 |]
           "Tele In 4",
           [| 744, 686, 63, 130
              447, 374, 89, 162
              0, 537, 103, 149
              1440, 374, 103, 157
              1302, 374, 138, 157
              1730, 198, 192, 163 |]
           "Tele Out 4",
           [| 1302, 374, 138, 157
              1440, 374, 103, 157
              0, 537, 103, 149
              447, 374, 89, 162
              744, 686, 63, 130 |]
           "TurnToIdle 4",
           [| 409, 686, 199, 131
              972, 0, 201, 180
              529, 0, 155, 194
              1543, 374, 203, 152
              341, 198, 154, 173
              788, 198, 138, 172
              825, 374, 185, 157
              184, 0, 184, 198 |]
           "Wake 4", [| 1599, 686, 142, 105; 181, 198, 160, 175 |] |]
