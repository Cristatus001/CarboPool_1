using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoleculeData
{
    public string moleculeName;
    public string formula;

    // Fragment counts
    public int ch3Count;
    public int ch2Count;
    public int chCount;
    public int cCount;
    public int ohCount;
    public int coohCount;
    public int nh2Count;

    // Structural order for ball spawning
    public Ball.FragmentType[] structuralOrder;

    public int TotalCorrectBalls =>
        ch3Count + ch2Count + chCount + cCount + ohCount + coohCount + nh2Count;
}

public static class MoleculeDatabase
{
    public static Dictionary<int, List<MoleculeData>> levelMolecules =
        new Dictionary<int, List<MoleculeData>>()
    {
        // LEVEL 1 - CHAIN ISOMERISM
        { 0, new List<MoleculeData>()
            {
                new MoleculeData {
                    moleculeName="n-Butane", formula="C₄H₁₀", ch3Count=2, ch2Count=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Isobutane", formula="C₄H₁₀", ch3Count=3, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH
                    }
                },
                new MoleculeData {
                    moleculeName="n-Pentane", formula="C₅H₁₂", ch3Count=2, ch2Count=3,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Isopentane", formula="C₅H₁₂", ch3Count=3, ch2Count=1, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Neopentane", formula="C₅H₁₂", ch3Count=4, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="n-Hexane", formula="C₆H₁₄", ch3Count=2, ch2Count=4,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methylpentane", formula="C₆H₁₄", ch3Count=3, ch2Count=2, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methylpentane", formula="C₆H₁₄", ch3Count=3, ch2Count=2, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2,2-Dimethylbutane", formula="C₆H₁₄", ch3Count=4, ch2Count=1, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2,3-Dimethylbutane", formula="C₆H₁₄", ch3Count=4, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="n-Heptane", formula="C₇H₁₆", ch3Count=2, ch2Count=5,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methylhexane", formula="C₇H₁₆", ch3Count=3, ch2Count=3, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methylhexane", formula="C₇H₁₆", ch3Count=3, ch2Count=3, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2,2-Dimethylpentane", formula="C₇H₁₆", ch3Count=4, ch2Count=2, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2,3-Dimethylpentane", formula="C₇H₁₆", ch3Count=4, ch2Count=1, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="1-Pentene", formula="C₅H₁₀", ch3Count=1, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Pentene", formula="C₅H₁₀", ch3Count=2, ch2Count=1, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methyl-1-butene", formula="C₅H₁₀", ch3Count=2, ch2Count=1, chCount=1, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methyl-2-butene", formula="C₅H₁₀", ch3Count=3, chCount=1, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="1-Hexene", formula="C₆H₁₂", ch3Count=1, ch2Count=3, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Hexene", formula="C₆H₁₂", ch3Count=2, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methyl-1-pentene", formula="C₆H₁₂", ch3Count=2, ch2Count=2, chCount=1, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="4-Methyl-1-pentene", formula="C₆H₁₂", ch3Count=2, ch2Count=1, chCount=3,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methyl-2-pentene", formula="C₆H₁₂", ch3Count=3, ch2Count=1, chCount=1, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methyl-2-pentene", formula="C₆H₁₂", ch3Count=3, ch2Count=1, chCount=1, cCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="n-Octane", formula="C₈H₁₈", ch3Count=2, ch2Count=6,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methylheptane", formula="C₈H₁₈", ch3Count=3, ch2Count=4, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methylheptane", formula="C₈H₁₈", ch3Count=3, ch2Count=4, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2,5-Dimethylhexane", formula="C₈H₁₈", ch3Count=4, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2,3-Dimethylpentane", formula="C₇H₁₆", ch3Count=4, ch2Count=1, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
            }
        },

        // LEVEL 2 - POSITION ISOMERISM (introduces OH)
        { 1, new List<MoleculeData>()
            {
                new MoleculeData {
                    moleculeName="Propan-1-ol", formula="C₃H₈O", ch3Count=1, ch2Count=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="Propan-2-ol", formula="C₃H₈O", ch3Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Butan-1-ol", formula="C₄H₁₀O", ch3Count=1, ch2Count=3, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="Butan-2-ol", formula="C₄H₁₀O", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-1-ol", formula="C₅H₁₂O", ch3Count=1, ch2Count=4, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-2-ol", formula="C₅H₁₂O", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-3-ol", formula="C₅H₁₂O", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hexan-1-ol", formula="C₆H₁₄O", ch3Count=1, ch2Count=5, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="Hexan-2-ol", formula="C₆H₁₄O", ch3Count=2, ch2Count=3, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hexan-3-ol", formula="C₆H₁₄O", ch3Count=2, ch2Count=3, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="1-Chloropropane", formula="C₃H₇Cl", ch3Count=1, ch2Count=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chloropropane", formula="C₃H₇Cl", ch3Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="1-Chlorobutane", formula="C₄H₉Cl", ch3Count=1, ch2Count=3, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chlorobutane", formula="C₄H₉Cl", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="1-Chloropentane", formula="C₅H₁₁Cl", ch3Count=1, ch2Count=4, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chloropentane", formula="C₅H₁₁Cl", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Chloropentane", formula="C₅H₁₁Cl", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="But-1-ene", formula="C₄H₈", ch3Count=1, ch2Count=1, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="But-2-ene", formula="C₄H₈", ch3Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pent-1-ene", formula="C₅H₁₀", ch3Count=1, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pent-2-ene", formula="C₅H₁₀", ch3Count=2, ch2Count=1, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propanone", formula="C₃H₆O", ch3Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Butanone", formula="C₄H₈O", ch3Count=2, ch2Count=1, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-2-one", formula="C₅H₁₀O", ch3Count=2, ch2Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-3-one", formula="C₅H₁₀O", ch3Count=2, ch2Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hexan-2-one", formula="C₆H₁₂O", ch3Count=2, ch2Count=3, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hexan-3-one", formula="C₆H₁₂O", ch3Count=2, ch2Count=3, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Heptan-2-one", formula="C₇H₁₄O", ch3Count=2, ch2Count=4, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hex-1-ene", formula="C₆H₁₂", ch3Count=1, ch2Count=3, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hex-2-ene", formula="C₆H₁₂", ch3Count=2, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
            }
        },

        // LEVEL 3 - FUNCTIONAL GROUP ISOMERISM (introduces COOH)
        { 2, new List<MoleculeData>()
            {
                new MoleculeData {
                    moleculeName="Propanal", formula="C₃H₆O", ch3Count=1, ch2Count=1, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH
                    }
                },
                new MoleculeData {
                    moleculeName="Propanone", formula="C₃H₆O", ch3Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Butanal", formula="C₄H₈O", ch3Count=1, ch2Count=2, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH
                    }
                },
                new MoleculeData {
                    moleculeName="Butanone", formula="C₄H₈O", ch3Count=2, ch2Count=1, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentanal", formula="C₅H₁₀O", ch3Count=1, ch2Count=3, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-2-one", formula="C₅H₁₀O", ch3Count=2, ch2Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-3-one", formula="C₅H₁₀O", ch3Count=2, ch2Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hexanal", formula="C₆H₁₂O", ch3Count=1, ch2Count=4, chCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH
                    }
                },
                new MoleculeData {
                    moleculeName="Hexan-2-one", formula="C₆H₁₂O", ch3Count=2, ch2Count=3, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Hexan-3-one", formula="C₆H₁₂O", ch3Count=2, ch2Count=3, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Butan-1-ol", formula="C₄H₁₀O", ch3Count=1, ch2Count=3, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="Butan-2-ol", formula="C₄H₁₀O", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethanoic acid", formula="C₂H₄O₂", ch3Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="Methyl methanoate", formula="C₂H₄O₂", ch3Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propanoic acid", formula="C₃H₆O₂", ch3Count=1, ch2Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="Ethyl methanoate", formula="C₃H₆O₂", ch3Count=1, ch2Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methyl ethanoate", formula="C₃H₆O₂", ch3Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Butanoic acid", formula="C₄H₈O₂", ch3Count=1, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="Methyl propanoate", formula="C₄H₈O₂", ch3Count=2, ch2Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethyl acetate", formula="C₄H₈O₂", ch3Count=2, ch2Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propyl methanoate", formula="C₄H₈O₂", ch3Count=1, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methoxypropane", formula="C₄H₁₀O", ch3Count=2, ch2Count=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethoxyethane", formula="C₄H₁₀O", ch3Count=2, ch2Count=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methoxybutane", formula="C₅H₁₂O", ch3Count=2, ch2Count=3, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-1-ol", formula="C₅H₁₂O", ch3Count=1, ch2Count=4, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-2-ol", formula="C₅H₁₂O", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Butyl methanoate", formula="C₅H₁₀O₂", ch3Count=1, ch2Count=3, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propyl acetate", formula="C₅H₁₀O₂", ch3Count=2, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethyl propanoate", formula="C₅H₁₀O₂", ch3Count=2, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentanoic acid", formula="C₅H₁₀O₂", ch3Count=1, ch2Count=3, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH
                    }
                },
            }
        },

        // LEVEL 4 - METAMERISM (introduces NH2)
        { 3, new List<MoleculeData>()
            {
                new MoleculeData {
                    moleculeName="Ethoxyethane", formula="C₄H₁₀O", ch3Count=2, ch2Count=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methoxypropane", formula="C₄H₁₀O", ch3Count=2, ch2Count=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethoxypropane", formula="C₅H₁₂O", ch3Count=2, ch2Count=3, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propoxypropane", formula="C₆H₁₄O", ch3Count=2, ch2Count=4, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methoxybutane", formula="C₅H₁₂O", ch3Count=2, ch2Count=3, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethoxybutane", formula="C₆H₁₄O", ch3Count=2, ch2Count=4, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-2-one", formula="C₅H₁₀O", ch3Count=2, ch2Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Pentan-3-one", formula="C₅H₁₀O", ch3Count=2, ch2Count=2, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methylbutan-2-one", formula="C₅H₁₀O", ch3Count=3, chCount=1, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethylmethylamine", formula="C₃H₉N", ch3Count=2, ch2Count=1, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Diethylamine", formula="C₄H₁₁N", ch3Count=2, ch2Count=2, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Dipropylamine", formula="C₆H₁₅N", ch3Count=2, ch2Count=4, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methylpropylamine", formula="C₄H₁₁N", ch3Count=2, ch2Count=2, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methylbutylamine", formula="C₅H₁₃N", ch3Count=2, ch2Count=3, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethylpropylamine", formula="C₅H₁₃N", ch3Count=2, ch2Count=3, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethylbutylamine", formula="C₆H₁₅N", ch3Count=2, ch2Count=4, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methyl propanoate", formula="C₄H₈O₂", ch3Count=2, ch2Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethyl methanoate", formula="C₃H₆O₂", ch3Count=1, ch2Count=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethyl propanoate", formula="C₅H₁₀O₂", ch3Count=2, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methyl ethanoate", formula="C₃H₆O₂", ch3Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propyl methanoate", formula="C₄H₈O₂", ch3Count=1, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Butyl methanoate", formula="C₅H₁₀O₂", ch3Count=1, ch2Count=3, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propyl ethanoate", formula="C₅H₁₀O₂", ch3Count=2, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="4-Methylpentan-2-one", formula="C₆H₁₂O", ch3Count=3, ch2Count=1, chCount=1, cCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3,3-Dimethylbutan-2-one", formula="C₆H₁₂O", ch3Count=4, cCount=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Dibutylamine", formula="C₈H₁₉N", ch3Count=2, ch2Count=6, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methoxyethane", formula="C₃H₈O", ch3Count=2, ch2Count=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Propyl butanoate", formula="C₇H₁₄O₂", ch3Count=2, ch2Count=4, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Methyl butanoate", formula="C₅H₁₀O₂", ch3Count=2, ch2Count=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Ethyl butanoate", formula="C₆H₁₂O₂", ch3Count=2, ch2Count=3, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
            }
        },

        // LEVEL 5 - GEOMETRICAL ISOMERISM
        { 4, new List<MoleculeData>()
            {
                new MoleculeData {
                    moleculeName="cis-2-Butene", formula="C₄H₈", ch3Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-2-Butene", formula="C₄H₈", ch3Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-1,2-Dichloroethene", formula="C₂H₂Cl₂", chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="trans-1,2-Dichloroethene", formula="C₂H₂Cl₂", chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="cis-2-Pentene", formula="C₅H₁₀", ch3Count=2, ch2Count=1, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-2-Pentene", formula="C₅H₁₀", ch3Count=2, ch2Count=1, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-3-Hexene", formula="C₆H₁₂", ch3Count=2, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-3-Hexene", formula="C₆H₁₂", ch3Count=2, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-2-Hexene", formula="C₆H₁₂", ch3Count=2, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-2-Hexene", formula="C₆H₁₂", ch3Count=2, ch2Count=2, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-1,2-Dibromoethene", formula="C₂H₂Br₂", chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="trans-1,2-Dibromoethene", formula="C₂H₂Br₂", chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="cis-Crotonic acid", formula="C₄H₆O₂", ch3Count=1, chCount=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="trans-Crotonic acid", formula="C₄H₆O₂", ch3Count=1, chCount=2, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="cis-2-Heptene", formula="C₇H₁₄", ch3Count=2, ch2Count=3, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-2-Heptene", formula="C₇H₁₄", ch3Count=2, ch2Count=3, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-3-Octene", formula="C₈H₁₆", ch3Count=2, ch2Count=4, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-3-Octene", formula="C₈H₁₆", ch3Count=2, ch2Count=4, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-2-Nonene", formula="C₉H₁₈", ch3Count=2, ch2Count=5, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-2-Nonene", formula="C₉H₁₈", ch3Count=2, ch2Count=5, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-2,3-Dimethyl-2-butene", formula="C₆H₁₂", ch3Count=4, cCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="trans-2,3-Dimethyl-2-butene", formula="C₆H₁₂", ch3Count=4, cCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.C,
                        Ball.FragmentType.C,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="cis-Decalin", formula="C₁₀H₁₈", ch2Count=8, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2
                    }
                },
                new MoleculeData {
                    moleculeName="trans-Decalin", formula="C₁₀H₁₈", ch2Count=8, chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2
                    }
                },
                new MoleculeData {
                    moleculeName="cis-cyclohexane-1,2-diol", formula="C₆H₁₂O₂", ch2Count=4, chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="trans-cyclohexane-1,2-diol", formula="C₆H₁₂O₂", ch2Count=4, chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="cis-cyclopentane-1,2-dicarboxylic acid", formula="C₇H₁₀O₄", ch2Count=3, chCount=2, coohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="trans-cyclopentane-1,2-dicarboxylic acid", formula="C₇H₁₀O₄", ch2Count=3, chCount=2, coohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.COOH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="cis-Stilbene", formula="C₁₄H₁₂", chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH
                    }
                },
                new MoleculeData {
                    moleculeName="trans-Stilbene", formula="C₁₄H₁₂", chCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH
                    }
                },
            }
        },

        // LEVEL 6 - OPTICAL ISOMERISM
        { 5, new List<MoleculeData>()
            {
                new MoleculeData {
                    moleculeName="2-Butanol (R)", formula="C₄H₁₀O", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Butanol (S)", formula="C₄H₁₀O", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chlorobutane (R)", formula="C₄H₉Cl", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chlorobutane (S)", formula="C₄H₉Cl", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="Lactic acid (R)", formula="C₃H₆O₃", ch3Count=1, chCount=1, ohCount=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="Lactic acid (S)", formula="C₃H₆O₃", ch3Count=1, chCount=1, ohCount=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="Glyceraldehyde (D)", formula="C₃H₆O₃", ch2Count=1, chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2
                    }
                },
                new MoleculeData {
                    moleculeName="Glyceraldehyde (L)", formula="C₃H₆O₃", ch2Count=1, chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2
                    }
                },
                new MoleculeData {
                    moleculeName="3-Chloropentane (R)", formula="C₅H₁₁Cl", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Chloropentane (S)", formula="C₅H₁₁Cl", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methylbutanoic acid (R)", formula="C₅H₁₀O₂", ch3Count=2, ch2Count=1, chCount=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methylbutanoic acid (S)", formula="C₅H₁₀O₂", ch3Count=2, ch2Count=1, chCount=1, coohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Aminobutane (R)", formula="C₄H₁₁N", ch3Count=2, ch2Count=1, chCount=1, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Aminobutane (S)", formula="C₄H₁₁N", ch3Count=2, ch2Count=1, chCount=1, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Bromobutane (R)", formula="C₄H₉Br", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Bromobutane (S)", formula="C₄H₉Br", ch3Count=2, ch2Count=1, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chloropentane (R)", formula="C₅H₁₁Cl", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chloropentane (S)", formula="C₅H₁₁Cl", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methyl-2-pentanol (R)", formula="C₆H₁₄O", ch3Count=3, ch2Count=1, chCount=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Methyl-2-pentanol (S)", formula="C₆H₁₄O", ch3Count=3, ch2Count=1, chCount=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methyl-1-butanol (R)", formula="C₅H₁₂O", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Methyl-1-butanol (S)", formula="C₅H₁₂O", ch3Count=2, ch2Count=2, chCount=1, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.OH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Aminobutane (R)", formula="C₄H₁₁N", ch3Count=2, ch2Count=1, chCount=1, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Chlorobutan-2-ol (R)", formula="C₄H₉ClO", ch3Count=2, chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="3-Chlorobutan-2-ol (S)", formula="C₄H₉ClO", ch3Count=2, chCount=2, ohCount=2,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Amino-3-methylbutanoic acid (R)", formula="C₅H₁₁NO₂", ch3Count=2, chCount=2, coohCount=1, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Amino-3-methylbutanoic acid (S)", formula="C₅H₁₁NO₂", ch3Count=2, chCount=2, coohCount=1, nh2Count=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.NH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.COOH
                    }
                },
                new MoleculeData {
                    moleculeName="2-Chloro-3-methylpentane (R)", formula="C₆H₁₃Cl", ch3Count=3, ch2Count=1, chCount=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Bromo-4-methylpentane (R)", formula="C₆H₁₃Br", ch3Count=3, ch2Count=1, chCount=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
                new MoleculeData {
                    moleculeName="2-Bromo-4-methylpentane (S)", formula="C₆H₁₃Br", ch3Count=3, ch2Count=1, chCount=2, ohCount=1,
                    structuralOrder = new Ball.FragmentType[] {
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.OH,
                        Ball.FragmentType.CH2,
                        Ball.FragmentType.CH,
                        Ball.FragmentType.CH3,
                        Ball.FragmentType.CH3
                    }
                },
            }
        },

    };

    public static MoleculeData GetRandomMolecule(int levelIndex)
    {
        if (!levelMolecules.ContainsKey(levelIndex)) return null;
        List<MoleculeData> molecules = levelMolecules[levelIndex];
        return molecules[Random.Range(0, molecules.Count)];
    }
}