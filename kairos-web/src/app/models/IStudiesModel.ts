import {ILabelModel} from "./ILabelModel";
import {IGroupModel} from "./IGroupModel";

export interface IStudiesModel{
  studiesId: number;
  studiesNumber: string;
  studiesTitle: string;
  studiesCreatedDate: string;
  studiesLabels?: ILabelModel[];
  group?: IGroupModel;
}
