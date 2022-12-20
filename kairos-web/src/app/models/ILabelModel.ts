import {IUserModel} from "./IUserModel";
import {IGroupModel} from "./IGroupModel";
import {IEventModel} from "./IEventModel";
import {IStudiesModel} from "./IStudiesModel";

export interface ILabelModel{
  labelId: number;
  labelTitle: string;
  user?: IUserModel;
  groups?: IGroupModel[];
  events?: IEventModel[];
  studies?: IStudiesModel[];
}
