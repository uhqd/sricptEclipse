//--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
//
//     CREATE OPTIMIZARION STRUCTURES FOR BREAST OR CHESTWALL (VARIAN ECLIPSE v15.x or later)
//
//     Based on https://github.com/VarianAPIs/
//     Example:   
//     /webinars & workshops/06 Apr 2018 Webinar/Eclipse Scripting API/Projects/CreateOptStructures/CreateOptStructures.cs
//     IUCT - ONCOPOLE   - JAN 2020
//
//
// Add to project references the two following files (available on any Eclipse station)
// C:\Program Files(x86)\Varian\RTM\15.6\esapi\API\VMS.TPS.Common.Model.API
// C:\Program Files(x86)\Varian\RTM\15.6\esapi\API\VMS.TPS.Common.Model.Types
//
//
//     ...           ...           ...           ...                                                                 
//    (- o)         (. .)         (- o)         (. .)                                                                
//ooO--(_)--Ooo-ooO--(_)--Ooo-ooO--(_)--Ooo-ooO--(_)--Ooo-
//
//--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*

//
/*
 
 * *  What this script does:
 *  The script allow to create optimiz. structures for breast or chestwall cancer
 *  1. Check if the physician's structures exist. If no there is either a warning message, either exit.
 *  2. Creates optimization structure. All PTV are converted to high resolution
 *  Warning : it is not possible to combine 2 structures with differnet resolution
 
 
  
  */

using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;


// This line is necessary to "write"
[assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
    public class Script
    {

        const string SCRIPT_NAME = "Opt Structures Script";

        // Structures declaration

        const string BODY_ID = "BODY";

        // - - -  existing structures in template "Sein Tomo v13"
        const string TRACHEE_ID = "Trachee";
        const string THYROIDE_ID = "Thyroide";
        const string SEIN_CONTRO_ID = "Sein contro";
        const string PTV_SUSCLV_ID = "PTV sus clav";
        const string PTV_SOUSCLV_ID = "PTV sous clav";
        const string PTV_SEIN_ID = "PTV sein";
        const string PTV_PAROI_ID = "PTV paroi";
        const string PTV_CMI_ID = "PTV cmi";
        const string PTV_BOOST_ID = "PTV boost";
        const string POUMONS_ID = "Poumons";
        const string POUMONG_ID = "PoumonGche";
        const string POUMOND_ID = "PoumonDt";
        const string PLEXUS_BRACHIAL_ID = "Plexus Brachial";
        const string OESOPHAGE_ID = "Oesophage";
        const string LARYNX_ID = "Larynx";
        const string FOIE_ID = "Foie";
        const string CTV_SUSCLAV_ID = "CTV sus clav";
        const string CTV_SOUSCLAV_ID = "CTV sous clav";
        const string CTV_SEIN_ID = "CTV sein";
        const string CTV_PAROI_ID = "CTV paroi";
        const string CTV_CMI_ID = "CTV cmi";
        const string COEUR_ID = "Coeur";
        const string CANAL_MED_ID = "Canal med";
        const string BOOST_LIT_T_ID = "Boost lit T";
        const string CTV_AXIL_ID = "CTV axillaire";
        const string TETE_HUM_G_ID = "TetehumeraleGche";
        const string TETE_HUM_D_ID = "TetehumeraleDte";
        const string PTV_AXIL_ID = "PTV Axillaire";
        const string PROTHESE_ID = "Prothese sein";


        // - - -  to create. See template "OPT_SEIN v13"
        const string RING_PTV_SEIN_ID = "RingPTVSein";
        const string RING_PTV_PAROI_ID = "RingPTVParoi";
        const string RINGGG_ID = "RingPTVgg";
        const string PTVSSUSCLV_OPT_ID = "PTVsusclavOPT";
        const string PTVSOUSCLV_OPT_ID = "PTVsousclavOPT";
        const string PTVSEINOPT_ID = "PTVseinOPT";
        const string PTVPAROIOPT_ID = "PTVparoiOPT";
        const string PTVCMIOPT_ID = "PTVcmiOPT";
        const string PTVBOOSTOPT_ID = "PTVboostOPT";
        const string CANAL_MED5_ID = "CanalMed+5";
        const string PROTHESE_MOINS_PTV_ID = "Prothese-PTV";
        const string PTVAXILOPT_ID = "PTVaxillaireOPT";
        const string PTVTOTAL_ID = "PTVTOTAL";
        //--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*



        public Script()
        {
        }

        public void Execute(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            Patient mypatient = context.Patient;
            mypatient.BeginModifications();


            //   Check if a patient and a structure set is loaded
            if (context.Patient == null || context.StructureSet == null)
            {
                MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // get the structure set
            StructureSet ss = context.StructureSet;

            // GET EXISTING STRUCTURES
            // IF THE STRUCTURE DOESN'T EXIST SCRIPT CAN BE STOPPED OR NOT ("return"= exit)

            Structure body = ss.Structures.FirstOrDefault(x => x.Id == BODY_ID);
            if (body == null)
            {

                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", BODY_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // stop the execution : 
                return;
            }
            Structure trachee = ss.Structures.FirstOrDefault(x => x.Id == TRACHEE_ID);
            if (trachee == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", TRACHEE_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure thyroide = ss.Structures.FirstOrDefault(x => x.Id == THYROIDE_ID);
            if (thyroide == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", THYROIDE_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure seincontro = ss.Structures.FirstOrDefault(x => x.Id == SEIN_CONTRO_ID);
            if (seincontro == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", SEIN_CONTRO_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure susclv = ss.Structures.FirstOrDefault(x => x.Id == PTV_SUSCLV_ID);
            if (susclv == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", PTV_SUSCLV_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure sousclv = ss.Structures.FirstOrDefault(x => x.Id == PTV_SOUSCLV_ID);
            if (sousclv == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", PTV_SOUSCLV_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure ptvsein = ss.Structures.FirstOrDefault(x => x.Id == PTV_SEIN_ID);
            Structure ptvparoi = ss.Structures.FirstOrDefault(x => x.Id == PTV_PAROI_ID);
            if (ptvparoi == null && ptvsein == null)
            {
                MessageBox.Show(string.Format("'{0}' or PTV Sein not found!", PTV_PAROI_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                return;
            }
            Structure ptvcmi = ss.Structures.FirstOrDefault(x => x.Id == PTV_CMI_ID);
            if (ptvcmi == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", PTV_CMI_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure ptvboost = ss.Structures.FirstOrDefault(x => x.Id == PTV_BOOST_ID);
            if (ptvboost == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", PTV_BOOST_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure poumonslow = ss.Structures.FirstOrDefault(x => x.Id == POUMONS_ID);
            if (poumonslow == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", POUMONS_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // Stop the execution
                return;
            }
            Structure poumong = ss.Structures.FirstOrDefault(x => x.Id == POUMONG_ID);
            if (poumong == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", POUMONG_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure poumond = ss.Structures.FirstOrDefault(x => x.Id == POUMOND_ID);
            if (poumond == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", POUMOND_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure plexusbrachial = ss.Structures.FirstOrDefault(x => x.Id == PLEXUS_BRACHIAL_ID);
            if (plexusbrachial == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", PLEXUS_BRACHIAL_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure oesophage = ss.Structures.FirstOrDefault(x => x.Id == OESOPHAGE_ID);
            if (oesophage == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", OESOPHAGE_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure larynx = ss.Structures.FirstOrDefault(x => x.Id == LARYNX_ID);
            if (larynx == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", LARYNX_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure foie = ss.Structures.FirstOrDefault(x => x.Id == FOIE_ID);
            if (foie == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", FOIE_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                return;
            }
            Structure ctvsusclav = ss.Structures.FirstOrDefault(x => x.Id == CTV_SUSCLAV_ID);
            if (ctvsusclav == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", CTV_SUSCLAV_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure ctvsousclav = ss.Structures.FirstOrDefault(x => x.Id == CTV_SOUSCLAV_ID);
            if (ctvsousclav == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", CTV_SOUSCLAV_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure ctvsein = ss.Structures.FirstOrDefault(x => x.Id == CTV_SEIN_ID);
            Structure ctvparoi = ss.Structures.FirstOrDefault(x => x.Id == CTV_PAROI_ID);
            if (ctvsein == null && ctvparoi == null)
            {
                //MessageBox.Show(string.Format("'{0}' or ctv paroi not found!", CTV_SEIN_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure ctvcmi = ss.Structures.FirstOrDefault(x => x.Id == CTV_CMI_ID);
            if (ctvcmi == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", CTV_CMI_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                // return;
            }
            Structure coeur = ss.Structures.FirstOrDefault(x => x.Id == COEUR_ID);
            if (coeur == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", COEUR_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                return;
            }
            Structure canalmed = ss.Structures.FirstOrDefault(x => x.Id == CANAL_MED_ID);
            if (canalmed == null)
            {
                // Warning message
                MessageBox.Show(string.Format("'{0}' not found!", CANAL_MED_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                return;
            }
            Structure boostlitt = ss.Structures.FirstOrDefault(x => x.Id == BOOST_LIT_T_ID);
            if (boostlitt == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", BOOST_LIT_T_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure ctvaxil = ss.Structures.FirstOrDefault(x => x.Id == CTV_AXIL_ID);
            if (ctvaxil == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", CTV_AXIL_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure tetehumeralg = ss.Structures.FirstOrDefault(x => x.Id == TETE_HUM_G_ID);
            if (tetehumeralg == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", TETE_HUM_G_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure tetehumerald = ss.Structures.FirstOrDefault(x => x.Id == TETE_HUM_D_ID);
            if (tetehumerald == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", TETE_HUM_D_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure axillaire = ss.Structures.FirstOrDefault(x => x.Id == PTV_AXIL_ID);
            if (axillaire == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", PTV_AXIL_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            Structure prothese = ss.Structures.FirstOrDefault(x => x.Id == PROTHESE_ID);
            if (prothese == null)
            {
                // Warning message
                // MessageBox.Show(string.Format("'{0}' not found!", PROTHESE_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }

            //--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            //      CREATION OF OPTIMIZATION STRUCTURES 
            //--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*

            Structure body3mm = ss.AddStructure("AVOIDANCE", "body3");  // body - 3 mm (deleted at the end)
            body3mm.SegmentVolume = body.Margin(-3.0);
            body3mm.ConvertToHighResolution();
            Structure poumons = ss.AddStructure("AVOIDANCE", "poumonsHigh");
            poumons.SegmentVolume = poumonslow.Margin(0.0); // copy poumons to be converted to high resolution
            poumons.ConvertToHighResolution();


            // PTV susclv OPT   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvSusClvOPt = ss.Structures.FirstOrDefault(x => x.Id == PTVSSUSCLV_OPT_ID);
            if (ptvSusClvOPt != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVSSUSCLV_OPT_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                ptvSusClvOPt = ss.AddStructure("PTV", PTVSSUSCLV_OPT_ID);
                ptvSusClvOPt.ConvertToHighResolution();
                Structure temp1 = ss.AddStructure("PTV", "temp1");
                temp1.SegmentVolume = susclv.Margin(0.0);
                temp1.ConvertToHighResolution();

                ptvSusClvOPt.SegmentVolume = temp1.And(body3mm); // crop 3 mm
                temp1.SegmentVolume = ptvSusClvOPt.Sub(poumons);
                ptvSusClvOPt.SegmentVolume = temp1.Margin(0.0);
                ss.RemoveStructure(temp1);
            }

            // PTV sousclv OPT   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvSousClvOPt = ss.Structures.FirstOrDefault(x => x.Id == PTVSOUSCLV_OPT_ID);
            if (ptvSousClvOPt != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVSOUSCLV_OPT_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                ptvSousClvOPt = ss.AddStructure("PTV", PTVSOUSCLV_OPT_ID);
                ptvSousClvOPt.ConvertToHighResolution();
                Structure temp2 = ss.AddStructure("PTV", "temp2");
                temp2.SegmentVolume = sousclv.Margin(0.0);
                temp2.ConvertToHighResolution();

                ptvSousClvOPt.SegmentVolume = temp2.And(body3mm); // crop 3 mm
                temp2.SegmentVolume = ptvSousClvOPt.Sub(poumons);
                ptvSousClvOPt.SegmentVolume = temp2.Margin(0.0);
                ss.RemoveStructure(temp2);
            }

            //  PTV SEIN OPT   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvSeinOpt = ss.Structures.FirstOrDefault(x => x.Id == PTVSEINOPT_ID);
            if (ptvSeinOpt != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVSEINOPT_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                if (ptvsein != null)   // if PTV sein exists. else do nothing
                {
                    ptvSeinOpt = ss.AddStructure("PTV", PTVSEINOPT_ID);
                    ptvSeinOpt.ConvertToHighResolution();
                    Structure ptvSeintemp = ss.AddStructure("PTV", "ptvseintemp");
                    ptvSeintemp.SegmentVolume = ptvsein.Margin(0.0);
                    ptvSeintemp.ConvertToHighResolution();

                    ptvSeinOpt.SegmentVolume = ptvSeintemp.And(body3mm); // crop 3 mm
                    Structure temp5 = ss.AddStructure("PTV", "temp5");
                    temp5.ConvertToHighResolution();
                    temp5.SegmentVolume = ptvSeinOpt.Sub(poumons);

                    ptvSeinOpt.SegmentVolume = temp5.Margin(0.0);
                    ss.RemoveStructure(temp5);
                    ss.RemoveStructure(ptvSeintemp);
                }
            }

            // PTV PAROI OPT   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvParoiOpt = ss.Structures.FirstOrDefault(x => x.Id == PTVPAROIOPT_ID);
            if (ptvParoiOpt != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVPAROIOPT_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                if (ptvParoiOpt != null)   // if PTV sein exists. else do nothing
                {
                    ptvParoiOpt = ss.AddStructure("PTV", PTVPAROIOPT_ID);
                    ptvParoiOpt.ConvertToHighResolution();
                    Structure ptvParoiTemp = ss.AddStructure("PTV", "ptvparoiemp");
                    ptvParoiTemp.SegmentVolume = ptvparoi.Margin(0.0);
                    ptvParoiTemp.ConvertToHighResolution();

                    ptvParoiOpt.SegmentVolume = ptvParoiTemp.And(body3mm); // crop 3 mm
                    Structure temp6 = ss.AddStructure("PTV", "temp6");
                    temp6.ConvertToHighResolution();
                    temp6.SegmentVolume = ptvParoiOpt.Sub(poumons);

                    ptvParoiOpt.SegmentVolume = temp6.Margin(0.0);
                    ss.RemoveStructure(temp6);
                    ss.RemoveStructure(ptvParoiTemp);
                }
            }

            // PTV CMIOPT   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvCMIopt = ss.Structures.FirstOrDefault(x => x.Id == PTVCMIOPT_ID);
            if (ptvCMIopt != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVCMIOPT_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                ptvCMIopt = ss.AddStructure("PTV", PTVCMIOPT_ID);
                ptvCMIopt.ConvertToHighResolution();
                Structure temp7 = ss.AddStructure("PTV", "temp7");
                temp7.SegmentVolume = ptvcmi.Margin(0.0);
                temp7.ConvertToHighResolution();

                ptvCMIopt.SegmentVolume = temp7.And(body3mm); // crop 3 mm
                temp7.SegmentVolume = ptvCMIopt.Sub(poumons);
                ptvCMIopt.SegmentVolume = temp7.Margin(0.0);
                ss.RemoveStructure(temp7);
            }

            // PTV BOOST OPT   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvBoostOpt = ss.Structures.FirstOrDefault(x => x.Id == PTVBOOSTOPT_ID);
            if (ptvBoostOpt != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVBOOSTOPT_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                ptvBoostOpt = ss.AddStructure("PTV", PTVBOOSTOPT_ID);
                ptvBoostOpt.ConvertToHighResolution();
                Structure temp8 = ss.AddStructure("PTV", "temp8");
                temp8.SegmentVolume = ptvboost.Margin(0.0);
                temp8.ConvertToHighResolution();

                ptvBoostOpt.SegmentVolume = temp8.And(body3mm); // crop 3 mm
                temp8.SegmentVolume = ptvBoostOpt.Sub(poumons);
                ptvBoostOpt.SegmentVolume = temp8.Margin(0.0);
                ss.RemoveStructure(temp8);


            }

            // PTV AXCILLAIRE OPT   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvAxilOpt = ss.Structures.FirstOrDefault(x => x.Id == PTVAXILOPT_ID);
            if (ptvAxilOpt != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVAXILOPT_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                ptvAxilOpt = ss.AddStructure("PTV", PTVAXILOPT_ID);
                ptvAxilOpt.ConvertToHighResolution();
                Structure temp9 = ss.AddStructure("PTV", "temp9");
                temp9.SegmentVolume = axillaire.Margin(0.0);
                temp9.ConvertToHighResolution();

                ptvAxilOpt.SegmentVolume = temp9.And(body3mm); // crop 3 mm
                temp9.SegmentVolume = ptvAxilOpt.Sub(poumons);
                ptvAxilOpt.SegmentVolume = temp9.Margin(0.0);
                ss.RemoveStructure(temp9);
            }

            // PTV TOTAL    *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure ptvTotal = ss.Structures.FirstOrDefault(x => x.Id == PTVTOTAL_ID);
            if (ptvTotal != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PTVTOTAL_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                ptvTotal = ss.AddStructure("PTV", PTVTOTAL_ID);
                ptvTotal.ConvertToHighResolution();

                Structure temp15 = ss.AddStructure("PTV", "temp15");
                temp15.ConvertToHighResolution();

                if (ptvAxilOpt != null)
                {
                    ptvTotal.SegmentVolume = ptvAxilOpt.Margin(0.0);
                    //temp15.SegmentVolume = ptvTotal.Margin(0.0);
                }
                if (ptvCMIopt != null)
                {
                    if (ptvTotal != null)
                    {
                        temp15.SegmentVolume = ptvTotal.Or(ptvCMIopt);
                        ptvTotal.SegmentVolume = temp15.Margin(0.0);
                    }
                    else
                        ptvTotal.SegmentVolume = ptvCMIopt.Margin(0.0);
                }
                if (ptvBoostOpt != null)
                {
                    if (ptvTotal != null)
                    {
                        temp15.SegmentVolume = ptvTotal.Or(ptvBoostOpt);
                        ptvTotal.SegmentVolume = temp15.Margin(0.0);
                    }
                    else
                        ptvTotal.SegmentVolume = ptvBoostOpt.Margin(0.0);
                }
                if (ptvSeinOpt != null)
                {
                    if (ptvTotal != null)
                    {
                        temp15.SegmentVolume = ptvTotal.Or(ptvSeinOpt);
                        ptvTotal.SegmentVolume = temp15.Margin(0.0);
                    }
                    else
                        ptvTotal.SegmentVolume = ptvSeinOpt.Margin(0.0);
                }
                if (ptvParoiOpt != null)
                {
                    if (ptvTotal != null)
                    {
                        temp15.SegmentVolume = ptvTotal.Or(ptvParoiOpt);
                        ptvTotal.SegmentVolume = temp15.Margin(0.0);
                    }
                    else
                        ptvTotal.SegmentVolume = ptvParoiOpt.Margin(0.0);
                }
                if (ptvSusClvOPt != null)
                {
                    if (ptvTotal != null)
                    {
                        temp15.SegmentVolume = ptvTotal.Or(ptvSusClvOPt);
                        ptvTotal.SegmentVolume = temp15.Margin(0.0);
                    }
                    else
                        ptvTotal.SegmentVolume = ptvSusClvOPt.Margin(0.0);
                }
                if (ptvSousClvOPt != null)
                {
                    if (ptvTotal != null)
                    {
                        temp15.SegmentVolume = ptvTotal.Or(ptvSousClvOPt);
                        ptvTotal.SegmentVolume = temp15.Margin(0.0);
                    }
                    else
                        ptvTotal.SegmentVolume = ptvSousClvOPt.Margin(0.0);
                }
                ss.RemoveStructure(temp15);
            }

            // canal +5   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure canal_5mm = ss.Structures.FirstOrDefault(x => x.Id == CANAL_MED5_ID);
            if (canal_5mm != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", CANAL_MED5_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                canal_5mm = ss.AddStructure("AVOIDANCE", CANAL_MED5_ID); // create the empty "canal + 5" structure

                canal_5mm.SegmentVolume = canalmed.Margin(5.0);
            }

            // Prothese-ptv   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure prothMinusPTV = ss.Structures.FirstOrDefault(x => x.Id == PROTHESE_MOINS_PTV_ID);
            if (prothMinusPTV != null)  // check if it already exists
            {
                //Warning message
                MessageBox.Show(string.Format("'{0}' already exists!", PROTHESE_MOINS_PTV_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // To stop the execution : 
                //return;
            }
            else
            {
                if (prothese != null) // else nothing
                {
                    if (ptvTotal != null) // else nothing
                    {
                        prothMinusPTV = ss.AddStructure("AVOIDANCE", PROTHESE_MOINS_PTV_ID);
                        prothMinusPTV.ConvertToHighResolution();
                        prothese.ConvertToHighResolution();
                        prothMinusPTV.SegmentVolume = prothese.Sub(ptvTotal);
                    }
                }
            }

            // ringptvsein   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            // Looking if a structure has already this name
            Structure ringptvsein = ss.Structures.FirstOrDefault(x => x.Id == RING_PTV_SEIN_ID);
            if (ringptvsein != null)  // check if it already exists
            {
                MessageBox.Show(string.Format("'{0}' already exists!", RING_PTV_SEIN_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (ptvSeinOpt != null)   // if PTV sein exists. else nothing
                {
                    ringptvsein = ss.AddStructure("AVOIDANCE", RING_PTV_SEIN_ID); // create ring ptv sein
                    ringptvsein.ConvertToHighResolution();

                    Structure tempring = ss.AddStructure("AVOIDANCE", "tempring"); // create a temp                  
                    Structure sein10mm = ss.AddStructure("AVOIDANCE", "sein10mm"); // create "sein+10"                    
                    tempring.ConvertToHighResolution();
                    sein10mm.ConvertToHighResolution();

                    sein10mm.SegmentVolume = ptvSeinOpt.Margin(10.0); // draw the "sein + 10"                   
                    tempring.SegmentVolume = sein10mm.Sub(ptvSeinOpt); // substract "ptv sein" from "sein+10" to create a ring
                    ringptvsein.SegmentVolume = tempring.Margin(0.0); // Ring <- tempring
                    if (ptvCMIopt != null)   // Substract CMI
                    {
                        ringptvsein.SegmentVolume = tempring.Sub(ptvCMIopt);
                        tempring.SegmentVolume = ringptvsein.Margin(0.0);
                    }
                    if (ptvSusClvOPt != null) // Substract susclv
                    {
                        ringptvsein.SegmentVolume = tempring.Sub(ptvSusClvOPt);
                        tempring.SegmentVolume = ringptvsein.Margin(0.0);
                    }
                    if (ptvSousClvOPt != null) // Substract sousclv
                    {
                        ringptvsein.SegmentVolume = tempring.Sub(ptvSousClvOPt);
                        tempring.SegmentVolume = ringptvsein.Margin(0.0);
                    }
                    if (ptvAxilOpt != null) // Substract axillaire
                    {
                        ringptvsein.SegmentVolume = tempring.Sub(ptvAxilOpt);
                        tempring.SegmentVolume = ringptvsein.Margin(0.0);
                    }
                    ss.RemoveStructure(sein10mm); // remove useless structures
                    ss.RemoveStructure(tempring); // remove useless structures
                }
            }


            // ringptvparoi    *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            // Looking if a structure has already this name
            Structure ringptvparoi = ss.Structures.FirstOrDefault(x => x.Id == RING_PTV_PAROI_ID);
            if (ringptvparoi != null)  // check if it already exists
            {
                MessageBox.Show(string.Format("'{0}' already exists!", RING_PTV_PAROI_ID), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (ptvParoiOpt != null)   // if PTV paroi exists. else do nothing
                {

                    ringptvparoi = ss.AddStructure("AVOIDANCE", RING_PTV_PAROI_ID); // create ring ptv sein
                    ringptvparoi.ConvertToHighResolution();

                    Structure tempring2 = ss.AddStructure("AVOIDANCE", "tempring2");
                    Structure paroi10mm = ss.AddStructure("AVOIDANCE", "paroi10mm"); // create "paroi+10"
                    tempring2.ConvertToHighResolution();
                    paroi10mm.ConvertToHighResolution();

                    paroi10mm.SegmentVolume = ptvParoiOpt.Margin(10.0); // draw the "paroi + 10"
                    tempring2.SegmentVolume = paroi10mm.Sub(ptvParoiOpt); // substract "ptv paroi" from "paroi+10" to create a ring
                                                                          // substract other PTVs 
                    ringptvparoi.SegmentVolume = tempring2.Margin(0.0); // Ring <- tempring
                    if (ptvCMIopt != null)
                    {
                        ringptvparoi.SegmentVolume = tempring2.Sub(ptvCMIopt); // Substract CMI
                        tempring2.SegmentVolume = ringptvparoi.Margin(0.0);
                    }
                    if (ptvSusClvOPt != null)
                    {
                        ringptvparoi.SegmentVolume = tempring2.Sub(ptvSusClvOPt);
                        tempring2.SegmentVolume = ringptvparoi.Margin(0.0);
                    }
                    if (ptvSousClvOPt != null)
                    {
                        ringptvparoi.SegmentVolume = tempring2.Sub(ptvSousClvOPt);
                        tempring2.SegmentVolume = ringptvparoi.Margin(0.0);
                    }
                    if (ptvAxilOpt != null)
                    {
                        ringptvparoi.SegmentVolume = tempring2.Sub(ptvAxilOpt);
                        tempring2.SegmentVolume = ringptvparoi.Margin(0.0);
                    }
                    // remove useless structures
                    ss.RemoveStructure(paroi10mm);
                    ss.RemoveStructure(tempring2);
                }
            }


            // ring GG   *--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-*--*
            Structure tempGG = ss.AddStructure("AVOIDANCE", "tempgg");  // sum of nodes (deleted at the end)
            Structure ringptvGG = ss.Structures.FirstOrDefault(x => x.Id == RINGGG_ID);

            if (ringptvGG != null)  // check if it already exists
            {
                MessageBox.Show(string.Format("'{0}' already exists!", ringptvGG), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {


                ringptvGG = ss.AddStructure("AVOIDANCE", RINGGG_ID);  // create ring ptv gg
                Structure tempring3 = ss.AddStructure("AVOIDANCE", "tempring3");
                Structure gg10mm = ss.AddStructure("AVOIDANCE", "gg10mm");

                tempGG.ConvertToHighResolution();
                ringptvGG.ConvertToHighResolution();
                tempring3.ConvertToHighResolution();
                gg10mm.ConvertToHighResolution();
                if (ptvCMIopt != null)
                {
                    tempGG.SegmentVolume = ptvCMIopt.Margin(0.0);
                }
                if (ptvSusClvOPt != null)
                {
                    if (tempGG != null)
                    {
                        tempring3.SegmentVolume = tempGG.Or(ptvSusClvOPt);
                        tempGG.SegmentVolume = tempring3.Margin(0.0);
                    }
                    else
                        tempGG.SegmentVolume = ptvSusClvOPt.Margin(0.0);
                }
                if (ptvSousClvOPt != null)
                {
                    if (tempGG != null)
                    {
                        tempring3.SegmentVolume = tempGG.Or(ptvSousClvOPt);
                        tempGG.SegmentVolume = tempring3.Margin(0.0);
                    }
                    else
                        tempGG.SegmentVolume = ptvSousClvOPt.Margin(0.0);
                }
                if (ptvAxilOpt != null)
                {
                    if (tempGG != null)
                    {
                        tempring3.SegmentVolume = tempGG.Or(ptvAxilOpt);
                        tempGG.SegmentVolume = tempring3.Margin(0.0);
                    }
                    else
                        tempGG.SegmentVolume = ptvAxilOpt.Margin(0.0);

                }
                if (tempGG != null) // else nothoing
                {
                    gg10mm.SegmentVolume = tempGG.Margin(10.0);  // gg +10
                    tempring3.SegmentVolume = gg10mm.Sub(tempGG); // create ring

                    if (ptvSeinOpt != null)
                        ringptvGG.SegmentVolume = tempring3.Sub(ptvSeinOpt); // substract sein or
                    if (ptvParoiOpt != null)
                        ringptvGG.SegmentVolume = tempring3.Sub(ptvParoiOpt); // substract paroi
                }
                ss.RemoveStructure(tempring3);
                ss.RemoveStructure(gg10mm);
            }

            //   DELETE USELESS STRUCTURES 
            ss.RemoveStructure(body3mm);
            ss.RemoveStructure(tempGG);
            ss.RemoveStructure(poumons); // high resolution lung
        }
    }
}




/*
 * 
 * 
 
 __    _  _   ___    ____  __  _  _   __   __ _ 
(  )  / )( \ / __)  / ___)(  )( \/ ) /  \ (  ( \
/ (_/\) \/ (( (__   \___ \ )( / \/ \(  O )/    /
\____/\____/ \___)  (____/(__)\_)(_/ \__/ \_)__)
 ____   __  ____   __                           
(___ \ /  \(___ \ /  \                          
 / __/(  0 )/ __/(  0 )                         
(____) \__/(____) \__/                          
     ...           ...           ...           ...                                                                 
    (- o)         (. .)         (- o)         (. .)                                                                
ooO--(_)--Ooo-ooO--(_)--Ooo-ooO--(_)--Ooo-ooO--(_)--Ooo-
 * 
 * 
 * 
 * Luc SIMON. 2020. 
 * IUCT-Oncopole - Toulouse - France
 * * * * */
