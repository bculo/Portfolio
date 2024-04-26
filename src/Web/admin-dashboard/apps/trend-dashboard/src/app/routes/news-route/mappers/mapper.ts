import { ArticleResDto } from "../../../shared/services/open-api/model/article-res-dto";
import { Article } from "../models/news.model";

export const mapToArticleArray = (input: ArticleResDto[]): Article[] => {
    return input.map(item => ({
        pageSource: item.pageSource,
        title: item.title,
        text: item.text,
        typeId: item.typeId,
        typeName: item.typeName,
        url: item.url,    
        id: item.id,
        imageUrl: item.searchWordImage   
    } as Article));    
};