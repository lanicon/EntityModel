// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TEntity">���ݶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public interface IBusinessLogicByStateData<TEntity, TPrimaryKey> : IUiBusinessLogicBase<TEntity, TPrimaryKey>
        where TEntity : EditDataObject, IIdentityData<TPrimaryKey>, IStateData, new()
    {
        #region ״̬����

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="id"></param>
        bool Reset(TPrimaryKey id);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Enable(TPrimaryKey id);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Disable(TPrimaryKey id);

        /// <summary>
        ///     ���ö���
        /// </summary>
        bool Discard(TPrimaryKey id);

        #endregion
    }
}