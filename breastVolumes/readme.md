
   CREATE OPTIMIZARION STRUCTURES FOR BREAST OR CHESTWALL (VARIAN ECLIPSE v15.x or later)
    Based on https://github.com/VarianAPIs/

Example based on:   
    /webinars & workshops/06 Apr 2018 Webinar/Eclipse Scripting API/Projects/CreateOptStructures/CreateOptStructures.cs
     IUCT - ONCOPOLE   - JAN 2020



 Add to project references the two following files (available on any Eclipse station)
 C:\Program Files(x86)\Varian\RTM\15.6\esapi\API\VMS.TPS.Common.Model.API
 C:\Program Files(x86)\Varian\RTM\15.6\esapi\API\VMS.TPS.Common.Model.Types


//     ...           ...           ...           ...                                                                 
//    (- o)         (. .)         (- o)         (. .)                                                                
//ooO--(_)--Ooo-ooO--(_)--Ooo-ooO--(_)--Ooo-ooO--(_)--Ooo-

 
 * *  What this script does:
 *  The script allow to create optimiz. structures for breast or chestwall cancer
 *  1. Check if the physician's structures exist. If no there is either a warning message, either exit.
 *  2. Creates optimization structure. All PTV are converted to high resolution
 *  Warning : it is not possible to combine 2 structures with differnet resolution
 
 
  
 
 
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
 
