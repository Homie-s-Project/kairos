import {IEventModel} from "./IEventModel";
import {ILabelModel} from "./ILabelModel";
import {IUserModel} from "./IUserModel";

export interface IGroupModel{
  groupId: number;
  groupName: string;
  groupIsPrivate: boolean;
  events?: IEventModel[];
  labels?: ILabelModel[];
  users?: IUserModel[];
}
