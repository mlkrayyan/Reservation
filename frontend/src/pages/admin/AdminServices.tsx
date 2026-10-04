import React, { useState, useEffect } from 'react';
import { Button } from '../../components/ui/Button';
import api, { getServices } from '../../services/api';
import './AdminServices.css';

interface ServiceRecord {
  id: string;
  name: string;
  category: string;
  durationMinutes: number;
  price: number;
  description: string;
}

export default function AdminServices() {
  const [services, setServices] = useState<ServiceRecord[]>([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingService, setEditingService] = useState<ServiceRecord | null>(null);

  // Form
  const [name, setName] = useState('');
  const [category, setCategory] = useState('MassageTherapy');
  const [durationMinutes, setDurationMinutes] = useState(60);
  const [price, setPrice] = useState(80);
  const [description, setDescription] = useState('');

  useEffect(() => {
    // Via the typed helper, not api.get: /services answers with a PagedResult
    // envelope of bilingual entities that this table cannot render directly.
    getServices()
      .then((data) => setServices(data.map((s) => ({ ...s, category: s.category ?? '', description: s.description ?? '' }))))
      .catch(() => {
        setServices([
          { id: 'srv-1', name: 'Royal Swedish Massage', category: 'MassageTherapy', durationMinutes: 60, price: 80, description: 'Deep muscle relaxation treatment using essential lavender oils.' },
          { id: 'srv-2', name: 'Hydra-Facial Glow', category: 'FacialSkincare', durationMinutes: 45, price: 65, description: 'Deep cleansing, exfoliation, and hydration for radiant skin.' },
          { id: 'srv-3', name: 'Luxury Spa Manicure & Pedicure', category: 'NailCare', durationMinutes: 50, price: 50, description: 'Nail shaping, cuticle care, scrub, and luxury polish.' },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const handleOpenAdd = () => {
    setEditingService(null);
    setName('');
    setCategory('MassageTherapy');
    setDurationMinutes(60);
    setPrice(80);
    setDescription('');
    setShowModal(true);
  };

  const handleOpenEdit = (srv: ServiceRecord) => {
    setEditingService(srv);
    setName(srv.name);
    setCategory(srv.category || 'MassageTherapy');
    setDurationMinutes(srv.durationMinutes);
    setPrice(srv.price);
    setDescription(srv.description);
    setShowModal(true);
  };

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault();

    if (editingService) {
      const updated = { ...editingService, name, category, durationMinutes, price, description };
      api.put(`/admin/services/${editingService.id}`, updated).catch(() => {});
      setServices((prev) => prev.map((s) => (s.id === editingService.id ? updated : s)));
    } else {
      const created: ServiceRecord = {
        id: `srv-${Date.now()}`,
        name,
        category,
        durationMinutes,
        price,
        description,
      };
      api.post('/admin/services', created).catch(() => {});
      setServices((prev) => [created, ...prev]);
    }
    setShowModal(false);
  };

  const handleDelete = (id: string) => {
    if (!window.confirm('Are you sure you want to delete this service?')) return;
    api.delete(`/admin/services/${id}`).catch(() => {});
    setServices((prev) => prev.filter((s) => s.id !== id));
  };

  return (
    <div className="admin-services-container">
      <div className="page-header flex-between">
        <div>
          <h1>Services Catalog</h1>
          <p className="subtitle">Add, edit, or adjust pricing & duration for spa treatments.</p>
        </div>
        <Button size="medium" variant="primary" onClick={handleOpenAdd}>
          + Add New Service
        </Button>
      </div>

      {loading ? (
        <div className="loader">Loading services...</div>
      ) : (
        <div className="table-card">
          <table className="admin-table">
            <thead>
              <tr>
                <th>Service Name</th>
                <th>Category</th>
                <th>Duration</th>
                <th>Price</th>
                <th>Description</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {services.map((srv) => (
                <tr key={srv.id}>
                  <td className="font-bold">{srv.name}</td>
                  <td><span className="cat-badge">{srv.category || 'General'}</span></td>
                  <td>⏱️ {srv.durationMinutes} mins</td>
                  <td className="font-bold text-accent">${srv.price}</td>
                  <td className="desc-cell">{srv.description}</td>
                  <td>
                    <div className="action-buttons">
                      <Button size="small" variant="secondary" onClick={() => handleOpenEdit(srv)}>
                        Edit
                      </Button>
                      <Button size="small" variant="danger" onClick={() => handleDelete(srv.id)}>
                        Delete
                      </Button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="service-modal-card" onClick={(e) => e.stopPropagation()}>
            <button className="close-btn" onClick={() => setShowModal(false)}>&times;</button>
            <h2>{editingService ? 'Edit Service' : 'Add New Service'}</h2>

            <form onSubmit={handleSave} className="admin-form">
              <div className="input-group">
                <label>Service Name</label>
                <input type="text" required value={name} onChange={(e) => setName(e.target.value)} />
              </div>

              <div className="input-group">
                <label>Category</label>
                <select value={category} onChange={(e) => setCategory(e.target.value)}>
                  <option value="HaircutStyling">Haircut & Styling</option>
                  <option value="FacialSkincare">Facial & Skincare</option>
                  <option value="MassageTherapy">Massage Therapy</option>
                  <option value="NailCare">Nail Care</option>
                  <option value="BodyTreatments">Body Treatments</option>
                  <option value="Waxing">Waxing</option>
                  <option value="MakeupBeauty">Makeup & Beauty</option>
                  <option value="EyelashEyebrow">Eyelashes & Eyebrows</option>
                </select>
              </div>

              <div className="form-row">
                <div className="input-group">
                  <label>Duration (Minutes)</label>
                  <input type="number" required value={durationMinutes} onChange={(e) => setDurationMinutes(Number(e.target.value))} />
                </div>
                <div className="input-group">
                  <label>Price ($ USD)</label>
                  <input type="number" required value={price} onChange={(e) => setPrice(Number(e.target.value))} />
                </div>
              </div>

              <div className="input-group">
                <label>Description</label>
                <textarea rows={3} value={description} onChange={(e) => setDescription(e.target.value)} />
              </div>

              <Button type="submit" size="large" variant="primary" style={{ width: '100%' }}>
                {editingService ? 'Update Service' : 'Create Service'}
              </Button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
