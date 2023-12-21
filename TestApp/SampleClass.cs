using GoShared.Helpers;
using GoShared.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GoPOS.Models;

[Table("#TABLE_NAME")]
public class #TABLE_NAME: IIdentifyEntity
{
#FIELDS_LIST
}