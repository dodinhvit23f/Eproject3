package abstracts;

import java.util.List;

import org.hibernate.Session;

import common.Hibernate;
import common.Utility;
import interfaces.BaseDaoInteface;

public abstract  class BaseDao<Clazz> implements BaseDaoInteface<Clazz>{
	protected Session session;
	private final Class<Clazz> clazz;
	
	public BaseDao(Class<Clazz> entity) {
		this.clazz = entity;
	}
		
	public List<Clazz> selectAll() {
		this.session = Hibernate.getConnection();
		List<Clazz> list = this.session
				.createQuery("select e from " +clazz.getSimpleName() + " e  ", this.clazz)
				.getResultList();
		Utility.closeObject(session);
		return list;
	}
        
        public Session getSession(){
            return Hibernate.getConnection();
        }
	
	protected abstract boolean vaildateRequest(Clazz clazz);
	protected abstract void setChange(Clazz oldclazz , Clazz newClazz);
	
	@Override
	public void insertOne(Clazz clazz){
		if(!vaildateRequest(clazz)) {
			return ;
		}
		this.session = Hibernate.getConnection();
		try {
			this.session.getTransaction().begin();
			this.session.persist(clazz);
			this.session.getTransaction().commit();
		} catch (Exception ex) {
			ex.printStackTrace();
		} finally {
			Utility.closeObject(session);
		}

	}
	
	@Override
	public void updateOne(Clazz clazz, int id) {
		if(!vaildateRequest(clazz)) {
			return ;
		}
		this.session = Hibernate.getConnection();
		try {
			this.session.getTransaction().begin();
			Clazz oldCalzz = this.session.find(this.clazz, id);
			setChange(oldCalzz,clazz);
			this.session.getTransaction().commit();
		} catch (Exception ex) {
			ex.printStackTrace();
		} finally {
			Utility.closeObject(session);
		}
	}

	@Override
	public void delete(String id) {
		this.session = Hibernate.getConnection();
		try {
			this.session.getTransaction().begin();
			this.session.delete(this.session.find(clazz, Utility.convertInt(id)));
					
			this.session.getTransaction().commit();
		} catch (Exception ex) {
			ex.printStackTrace();
		} finally {
			Utility.closeObject(session);
		}
	}
        
        public Object getTotalPage(int limmit){
            this.session = Hibernate.getConnection();
                 List <Object> object = null;
		try {
			this.session.getTransaction().begin();
			object = this.session
				.createNativeQuery("select count(*)/(?1) as page from  "+clazz.getSimpleName())
                                .setParameter(1, limmit)
                                .getResultList();					
			this.session.getTransaction().commit();
		} catch (Exception ex) {
			ex.printStackTrace();
		} finally {
			Utility.closeObject(session);
		}
                if(object == null || object.isEmpty() ){
                    return 1;
                }
                return object.get(0);
        }
}
