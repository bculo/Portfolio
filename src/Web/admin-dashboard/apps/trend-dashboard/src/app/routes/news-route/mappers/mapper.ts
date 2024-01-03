import { ArticleTypeDto } from "../../../shared/services/open-api";
import { Article } from "../models/news.model";

export const mapToArticleArray = (input: ArticleTypeDto[]): Article[] => {
    return input.map(item => ({
        pageSource: item.pageSource,
        title: item.title,
        text: item.text,
        typeId: item.typeId,
        typeName: item.typeName,
        url: item.url,    
        id: item.id   
    } as Article));    
};